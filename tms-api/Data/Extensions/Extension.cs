using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Data.Extensions
{
    /// <summary>
    /// Hierarchy node class which contains a nested collection of hierarchy nodes
    /// </summary>
    /// <typeparam name="T">Entity</typeparam>
    public class HierarchyNode<T> where T : class
    {
        public HierarchyNode()
        {
            ChildNodes = new List<HierarchyNode<T>>();
        }
        public T Entity { get; set; }
        public IEnumerable<HierarchyNode<T>> ChildNodes { get; set; }
        public int Depth { get; set; }
        public bool HasChildren
        {
            get { return ChildNodes.Any(); }
        }
        public T Parent { get; set; }
    }
    public static class Extension
    {
        public static IEnumerable<Tuple<T, int>> ToLeveled<T>(this IEnumerable<T> source, int level)
        {
            if (source == null)
            {
                return null;
            }
            else
            {
                return source.Select(item => new Tuple<T, int>(item, level));
            }
        }
        public static IEnumerable<(T, int[])> Expand<T>(this IEnumerable<T> source, Func<T, IEnumerable<T>> getChildren)
        {
            var stack = new Stack<IEnumerator<T>>();
            var e = source.GetEnumerator();
            List<int> indexes = new List<int>() { -1 };
            try
            {
                while (true)
                {
                    while (e.MoveNext())
                    {
                        var item = e.Current;
                        indexes[stack.Count]++;
                        yield return (item, indexes.Take(stack.Count + 1).ToArray());
                        var elements = getChildren(item);
                        if (elements == null) continue;
                        stack.Push(e);
                        e = elements.GetEnumerator();
                        if (indexes.Count == stack.Count)
                            indexes.Add(-1);
                    }
                    if (stack.Count == 0) break;
                    e.Dispose();
                    indexes[stack.Count] = -1;
                    e = stack.Pop();
                }
            }
            finally
            {
                e.Dispose();
                while (stack.Count != 0) stack.Pop().Dispose();
            }
        }
        public static IEnumerable<Tuple<T, int>> FlattenWithLevel<T>(
           this IEnumerable<T> items,
           Func<T, IEnumerable<T>> getChilds)
        {
            var stack = new Stack<Tuple<T, int>>();
            foreach (var item in items)
                stack.Push(new Tuple<T, int>(item, 1));

            while (stack.Count > 0)
            {
                var current = stack.Pop();
                yield return current;
                foreach (var child in getChilds(current.Item1))
                    stack.Push(new Tuple<T, int>(child, current.Item2 + 1));
            }
        }
        public static IEnumerable<T> TraverseAndFlatten<T, V>(this IEnumerable<T> items, Func<T, IEnumerable<T>> nested, Func<T, V> orderBy)
        {
            var stack = new Stack<T>();
            foreach (var item in items.OrderBy(orderBy))
                stack.Push(item);

            while (stack.Count > 0)
            {
                var current = stack.Pop();
                yield return current;

                var children = nested(current).OrderBy(orderBy);
                if (children == null) continue;

                foreach (var child in children)
                    stack.Push(child);
            }
        }
        public static IEnumerable<T> Flatten<T>(
          this IEnumerable<T> source,
          Func<T, IEnumerable<T>> childSelector)
        {
            HashSet<T> added = new HashSet<T>();
            Queue<T> queue = new Queue<T>();
            foreach (T t in source)
                if (added.Add(t))
                    queue.Enqueue(t);
            while (queue.Count > 0)
            {
                T current = queue.Dequeue();
                yield return current;
                if (current != null)
                {
                    IEnumerable<T> children = childSelector(current);
                    if (children != null)
                        foreach (T t in childSelector(current))
                            if (added.Add(t))
                                queue.Enqueue(t);
                }
            }
        }
        private static IEnumerable<HierarchyNode<TEntity>>
     CreateHierarchy<TEntity, TProperty>(
       IEnumerable<TEntity> allItems,
       TEntity parentItem,
       Func<TEntity, TProperty> idProperty,
       Func<TEntity, TProperty> parentIdProperty,
       object rootItemId,
       int maxDepth,
       int depth) where TEntity : class
        {
            IEnumerable<TEntity> childs;

            if (rootItemId != null)
            {
                childs = allItems.Where(i => idProperty(i).Equals(rootItemId));
            }
            else
            {
                if (parentItem == null)
                {
                    childs = allItems.Where(i => parentIdProperty(i).Equals(default(TProperty)));
                }
                else
                {
                    childs = allItems.Where(i => parentIdProperty(i).Equals(idProperty(parentItem)));
                }
            }

            if (childs.Count() > 0)
            {
                depth++;

                if ((depth <= maxDepth) || (maxDepth == 0))
                {
                    foreach (var item in childs)
                        yield return
                          new HierarchyNode<TEntity>()
                          {
                              Entity = item,
                              ChildNodes =
                                CreateHierarchy(allItems.AsEnumerable(), item, idProperty, parentIdProperty, null, maxDepth, depth),
                              Depth = depth,
                              Parent = parentItem
                          };
                }
            }
        }

        /// <summary>
        /// LINQ to Objects (IEnumerable) AsHierachy() extension method
        /// </summary>
        /// <typeparam name="TEntity">Entity class</typeparam>
        /// <typeparam name="TProperty">Property of entity class</typeparam>
        /// <param name="allItems">Flat collection of entities</param>
        /// <param name="idProperty">Func delegete to Id/Key of entity</param>
        /// <param name="parentIdProperty">Func delegete to parent Id/Key</param>
        /// <returns>Hierarchical structure of entities</returns>
        public static IEnumerable<HierarchyNode<TEntity>> AsHierarchy<TEntity, TProperty>(
          this IEnumerable<TEntity> allItems,
          Func<TEntity, TProperty> idProperty,
          Func<TEntity, TProperty> parentIdProperty) where TEntity : class
        {
            return CreateHierarchy(allItems, default(TEntity), idProperty, parentIdProperty, null, 0, 0);
        }

        /// <summary>
        /// LINQ to Objects (IEnumerable) AsHierachy() extension method
        /// </summary>
        /// <typeparam name="TEntity">Entity class</typeparam>
        /// <typeparam name="TProperty">Property of entity class</typeparam>
        /// <param name="allItems">Flat collection of entities</param>
        /// <param name="idProperty">Func delegete to Id/Key of entity</param>
        /// <param name="parentIdProperty">Func delegete to parent Id/Key</param>
        /// <param name="rootItemId">Value of root item Id/Key</param>
        /// <returns>Hierarchical structure of entities</returns>
        public static IEnumerable<HierarchyNode<TEntity>> AsHierarchy<TEntity, TProperty>(
          this IEnumerable<TEntity> allItems,
          Func<TEntity, TProperty> idProperty,
          Func<TEntity, TProperty> parentIdProperty,
          object rootItemId) where TEntity : class
        {
            return CreateHierarchy(allItems, default(TEntity), idProperty, parentIdProperty, rootItemId, 0, 0);
        }

        /// <summary>
        /// LINQ to Objects (IEnumerable) AsHierachy() extension method
        /// </summary>
        /// <typeparam name="TEntity">Entity class</typeparam>
        /// <typeparam name="TProperty">Property of entity class</typeparam>
        /// <param name="allItems">Flat collection of entities</param>
        /// <param name="idProperty">Func delegete to Id/Key of entity</param>
        /// <param name="parentIdProperty">Func delegete to parent Id/Key</param>
        /// <param name="rootItemId">Value of root item Id/Key</param>
        /// <param name="maxDepth">Maximum depth of tree</param>
        /// <returns>Hierarchical structure of entities</returns>
        public static IEnumerable<HierarchyNode<TEntity>> AsHierarchy<TEntity, TProperty>(
          this IEnumerable<TEntity> allItems,
          Func<TEntity, TProperty> idProperty,
          Func<TEntity, TProperty> parentIdProperty,
          object rootItemId,
          int maxDepth) where TEntity : class
        {
            return CreateHierarchy(allItems, default(TEntity), idProperty, parentIdProperty, rootItemId, maxDepth, 0);
        }

        private static IEnumerable<HierarchyNode<TEntity>>
    CreateHierarchy<TEntity>(IQueryable<TEntity> allItems,
      TEntity parentItem,
      string propertyNameId,
      string propertyNameParentId,
      object rootItemId,
      int maxDepth,
      int depth) where TEntity : class
        {
            ParameterExpression parameter = Expression.Parameter(typeof(TEntity), "e");
            Expression<Func<TEntity, bool>> predicate;

            if (rootItemId != null)
            {
                Expression left = Expression.Property(parameter, propertyNameId);
                left = Expression.Convert(left, rootItemId.GetType());
                Expression right = Expression.Constant(rootItemId);

                predicate = Expression.Lambda<Func<TEntity, bool>>(Expression.Equal(left, right), parameter);
            }
            else
            {
                if (parentItem == null)
                {
                    predicate =
                      Expression.Lambda<Func<TEntity, bool>>(
                        Expression.Equal(Expression.Property(parameter, propertyNameParentId),
                                         Expression.Constant(null)), parameter);
                }
                else
                {
                    Expression left = Expression.Property(parameter, propertyNameParentId);
                    left = Expression.Convert(left, parentItem.GetType().GetProperty(propertyNameId).PropertyType);
                    Expression right = Expression.Constant(parentItem.GetType().GetProperty(propertyNameId).GetValue(parentItem, null));

                    predicate = Expression.Lambda<Func<TEntity, bool>>(Expression.Equal(left, right), parameter);
                }
            }

            IEnumerable<TEntity> childs = allItems.Where(predicate).ToList();

            if (childs.Count() > 0)
            {
                depth++;

                if ((depth <= maxDepth) || (maxDepth == 0))
                {
                    foreach (var item in childs)
                        yield return
                          new HierarchyNode<TEntity>()
                          {
                              Entity = item,
                              ChildNodes =
                              CreateHierarchy(allItems, item, propertyNameId, propertyNameParentId, null, maxDepth, depth),
                              Depth = depth,
                              Parent = parentItem
                          };
                }
            }
        }

        /// <summary>
        /// LINQ to SQL (IQueryable) AsHierachy() extension method
        /// </summary>
        /// <typeparam name="TEntity">Entity class</typeparam>
        /// <param name="allItems">Flat collection of entities</param>
        /// <param name="propertyNameId">String with property name of Id/Key</param>
        /// <param name="propertyNameParentId">String with property name of parent Id/Key</param>
        /// <returns>Hierarchical structure of entities</returns>
        public static IEnumerable<HierarchyNode<TEntity>> AsHierarchy<TEntity>(
          this IQueryable<TEntity> allItems,
          string propertyNameId,
          string propertyNameParentId) where TEntity : class
        {
            return CreateHierarchy(allItems, null, propertyNameId, propertyNameParentId, null, 0, 0);
        }

        /// <summary>
        /// LINQ to SQL (IQueryable) AsHierachy() extension method
        /// </summary>
        /// <typeparam name="TEntity">Entity class</typeparam>
        /// <param name="allItems">Flat collection of entities</param>
        /// <param name="propertyNameId">String with property name of Id/Key</param>
        /// <param name="propertyNameParentId">String with property name of parent Id/Key</param>
        /// <param name="rootItemId">Value of root item Id/Key</param>
        /// <returns>Hierarchical structure of entities</returns>
        public static IEnumerable<HierarchyNode<TEntity>> AsHierarchy<TEntity>(
          this IQueryable<TEntity> allItems,
          string propertyNameId,
          string propertyNameParentId,
          object rootItemId) where TEntity : class
        {
            return CreateHierarchy(allItems, null, propertyNameId, propertyNameParentId, rootItemId, 0, 0);
        }

        /// <summary>
        /// LINQ to SQL (IQueryable) AsHierachy() extension method
        /// </summary>
        /// <typeparam name="TEntity">Entity class</typeparam>
        /// <param name="allItems">Flat collection of entities</param>
        /// <param name="propertyNameId">String with property name of Id/Key</param>
        /// <param name="propertyNameParentId">String with property name of parent Id/Key</param>
        /// <param name="rootItemId">Value of root item Id/Key</param>
        /// <param name="maxDepth">Maximum depth of tree</param>
        /// <returns>Hierarchical structure of entities</returns>
        public static IEnumerable<HierarchyNode<TEntity>> AsHierarchy<TEntity>(
          this IQueryable<TEntity> allItems,
          string propertyNameId,
          string propertyNameParentId,
          object rootItemId,
          int maxDepth) where TEntity : class
        {
            return CreateHierarchy(allItems, null, propertyNameId, propertyNameParentId, rootItemId, maxDepth, 0);
        }
    }

}

