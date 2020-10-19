import { Component, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { FilterSettingsModel, TreeGridComponent } from '@syncfusion/ej2-angular-treegrid';
import { ClientRouter } from 'src/app/_core/enum/ClientRouter';
import { JobType, PeriodType } from 'src/app/_core/enum/task.enum';
import { AddTask } from 'src/app/_core/_model/add.task';
import { Task } from 'src/app/_core/_model/Task';
import { AddTaskService } from 'src/app/_core/_service/addTask.service';
import { AlertifyService } from 'src/app/_core/_service/alertify.service';
import { JobTypeService } from 'src/app/_core/_service/jobType.service';
import { ProjectDetailService } from 'src/app/_core/_service/projectDetail.service';
import { RoutineService } from 'src/app/_core/_service/routine.service';
import { CommentComponent } from '../../modals/comment/comment.component';
import { AddTaskModalComponent } from '../add-task-modal/add-task-modal.component';
import { TutorialModalComponent } from '../tutorial-modal/tutorial-modal.component';
import { WatchTutorialVideoComponent } from '../watch-tutorial-video/watch-tutorial-video.component';

@Component({
  selector: 'app-routine-child',
  templateUrl: './routine-child.component.html',
  styleUrls: ['./routine-child.component.css']
})
export class RoutineChildComponent implements OnInit {
  taskCode: string;
  data: any;
  @ViewChild('treegridTasks')
  public treeGridObj: TreeGridComponent;
  public ocs: object;
  public srcTutorial: string;
  public taskId: number;
  public showTasks: boolean;
  public parentId: number;
  public toolbarOptions: any[];
  public toolbarOptionsTasks: any[];
  public pageSetting: object;
  public listOCs = JSON.parse(localStorage.getItem('user')).User.ListOCs;
  public ocLevel = JSON.parse(localStorage.getItem('user')).User.OCLevel;
  public isLeader = JSON.parse(localStorage.getItem('user')).User.IsLeader;
  public currentUser = JSON.parse(localStorage.getItem('user')).User.ID;
  public contextMenuItems: object[];
  public filterSettings: FilterSettingsModel;
  private tutorialName: string;
  searchSettings: object;
  ocID: any;
  ocId: any;
  constructor(private route: ActivatedRoute,
              private modalService: NgbModal,
              private addTaskService: AddTaskService,
              private jobtypeService: JobTypeService,
              private projectDetailService: ProjectDetailService,
              private alertify: AlertifyService,
              private routineService: RoutineService) { 
                this.optionGridTree();
                this.checkRole();
              }

  ngOnInit() {
    this.route.data.subscribe(data => {
      this.taskCode = this.route.snapshot.paramMap.get('code');
      this.getData();
    });
  }
  getData() {
    this.routineService.routineChild(this.taskCode)
      .subscribe(arg => this.data = arg);
  }
  optionGridTree() {
    this.filterSettings = { type: 'CheckBox' };
    this.pageSetting = { pageCount: 5, pageSizes: true };
    this.toolbarOptions = [
      'Search',
      'ExpandAll',
      'CollapseAll'
    ];
    this.searchSettings = {
      hierarchyMode: 'Parent',
      fields: ['Entity.JobName'],
      operator: 'contains',
      key: '',
      ignoreCase: true
    };
  }
  checkRole() {
    if (this.ocLevel >= 3 && !this.isLeader) {
      this.toolbarOptionsTasks = [
        'Search',
        'ExpandAll',
        'CollapseAll',
        'ExcelExport',
        'Print',
        { text: 'All columns', tooltipText: 'Show all columns', prefixIcon: 'e-add', id: 'AllColumns' },
        { text: 'Default columns', tooltipText: 'Show default columns', prefixIcon: 'e-add', id: 'DefaultColumns' },
      ];
      this.contextMenuItems = [
        {
          text: 'Add Tutorial Video',
          iconCss: 'fa fa-plus-square',
          target: '.e-content',
          id: 'Tutorial'
        },
        {
          text: 'Edit Tutorial',
          iconCss: 'fa fa-wrench',
          target: '.e-content',
          id: 'EditTutorial'
        },
        {
          text: 'Watch Video',
          iconCss: 'fa fa-play',
          target: '.e-content',
          id: 'WatchVideo'
        },
        {
          text: 'Follow',
          iconCss: ' fa fa-bell',
          target: '.e-content',
          id: 'Follow'
        },
      ];
    } else {
      this.toolbarOptionsTasks = [
        // { text: 'Add New', tooltipText: 'Add New', prefixIcon: 'e-add', id: 'CreateNew' },
        'Search',
        'ExpandAll',
        'CollapseAll',
        'ExcelExport',
        'Print',
        // { text: 'All columns', tooltipText: 'Show all columns', prefixIcon: 'e-add', id: 'AllColumns' },
        // { text: 'Default columns', tooltipText: 'Show default columns', prefixIcon: 'e-add', id: 'DefaultColumns' },
      ];
      this.contextMenuItems = [
        {
          text: 'Add Sub-Task',
          iconCss: ' e-icons e-add',
          target: '.e-content',
          id: 'Add-Sub-Task'
        },
        {
          text: 'Edit',
          iconCss: ' e-icons e-edit',
          target: '.e-content',
          id: 'EditTask'
        },
        {
          text: 'Delete',
          iconCss: ' e-icons e-delete',
          target: '.e-content',
          id: 'DeleteTask'
        },
        {
          text: 'Add Tutorial Video',
          iconCss: 'fa fa-plus-square',
          target: '.e-content',
          id: 'Tutorial'
        },
        {
          text: 'Edit Tutorial',
          iconCss: 'fa fa-wrench',
          target: '.e-content',
          id: 'EditTutorial'
        },
        {
          text: 'Watch Video',
          iconCss: 'fa fa-play',
          target: '.e-content',
          id: 'WatchVideo'
        },
        {
          text: 'Follow',
          iconCss: ' fa fa-bell',
          target: '.e-content',
          id: 'Follow'
        },
        {
          text: 'Unfollow',
          iconCss: ' fa fa-bell-slash',
          target: '.e-content',
          id: 'Unfollow'
        }
      ];
    }
  }
  recordDoubleClick(agrs?: any) {
    this.openCommentModal(agrs);
  }

  contextMenuClick(args) {
    console.log('contextMenuClick', args);
    const data = args.rowInfo.rowData.Entity;
    // console.log('contextMenuClickdata', data);

    this.taskId = data.ID;
    this.tutorialName = data.JobName;
    this.srcTutorial = data.VideoLink;
    switch (args.item.id) {
      case 'Add-Sub-Task':
        this.parentId = data.ID;
        this.openAddSubTaskModal();
        break;
      case 'Tutorial':
        this.openTutorialModal(args);
        break;
      case 'EditTutorial':
        this.openEditTutorialModal(args);
        break;
      case 'WatchVideo':
        this.openWatchTutorialWatchModal();
        break;
      case 'EditTask':
        this.openEditTaskModal(args);
        break;
      case 'DeleteTask':
        this.delete();
        break;
    }
  }
  toolbarClick(args) {
    switch (args.item.text) {
      case 'PDF Export':
        this.treeGridObj.pdfExport({ hierarchyExportMode: 'All' });
        break;
      case 'Excel Export':
        this.treeGridObj.excelExport({ hierarchyExportMode: 'All' });
        break;
      case 'Add New':
        this.openAddMainTaskModal();
        break;
      case 'All columns':
        this.showAllColumnsTreegrid();
        break;
      case 'Default columns':
        this.defaultColumnsTreegrid();
        break;
    }
   }
  defaultColumnsTreegrid() {
    const show = ['Follow', 'Priority', 'From', 'Task Name',
      'Created Date', 'Finished DateTime',
      'PIC', 'Status', 'Deputy', 'Watch Video', 'Period Type'];
    for (const item of show) {
      this.treeGridObj.showColumns([item, 'Ship Name']);
    }
  }
  showAllColumnsTreegrid() {
    const hide = ['Follow', 'Priority', 'From', 'Status', 'Watch Video', 'Deputy', 'Period Type'];
    for (const item of hide) {
      this.treeGridObj.hideColumns([item, 'Ship Name']);
    }
  }
  openAddMainTaskModal() {
    const modalRef = this.modalService.open(AddTaskModalComponent, { size: 'xl' });
    modalRef.componentInstance.title = 'Add Routine Main Task';
    modalRef.componentInstance.ocid = this.ocId;
    modalRef.componentInstance.jobType = JobType.Routine;
    modalRef.result.then((result) => {
      // console.log('openAddMainTaskModal', result)
    }, (reason) => {
    });
    this.jobtypeService.changeMessage(JobType.Routine);
  }
  dataBound(args) { }
  periodText(enumVal) {
    return this.getEnumKeyByEnumValue(PeriodType, Number(enumVal));
  }
  getEnumKeyByEnumValue(myEnum, enumValue) {
    const keys = Object.keys(myEnum).filter(x => myEnum[x] === enumValue);
    return keys.length > 0 ? keys[0] : null;
  }

  contextMenuOpen(args) {
   }
  delete() {
    if (this.taskId > 0) {
      this.alertify.confirm(
        'Delete Routine Task',
        'Are you sure you want to delete this ProjectID "' + this.taskId + '" ?',
        () => {
          this.projectDetailService.delete(this.taskId).subscribe(
            (res: any) => {
              if (res.status === -1) {
               this.alertify.warning(res.message, true);
              }
              if (res.status === 1) {
                this.alertify.success(res.message);
                // this.getTasks();
               }
              if (res.status === 0) {
                this.alertify.warning(res.message, true);
               }
            },
            error => {
              this.alertify.error('Failed to delete the Project');
            }
          );
        }
      );
    }
  }
  follow(id) {
    this.routineService.follow(id).subscribe(res => {
      this.alertify.success('You have already followd this one!');
      // this.getTasks();
      this.addTaskService.changeMessage(new AddTask(JobType.Routine, this.ocId));
    });
  }
  private editTask(args?): Task {
    if ((args || null) === null) {
      return null;
    }
    const data = args.rowInfo.rowData.Entity;
    return new Task()
      .create(
        data.ID,
        data.JobName,
        data.PICs,
        data.FromWho.ID,
        data.ProjectID,
        false,
        data.PriorityID,
        data.ParentID,
        data.periodType,
        data.ProjectID,
        data.JobTypeID,
        data.Deputies,
        data.OCID,
        data.DueDate
      );
  }
  openAddSubTaskModal() {
    const modalRef = this.modalService.open(AddTaskModalComponent, { size: 'xl' });
    modalRef.componentInstance.title = 'Add Routine Sub-Task';
    modalRef.componentInstance.ocid = this.ocId;
    modalRef.componentInstance.parentId = this.parentId;
    modalRef.componentInstance.jobType = JobType.Routine;
    modalRef.result.then((result) => {
      // console.log('openAddSubTaskModal', result);
    }, (reason) => {
    });
    this.jobtypeService.changeMessage(JobType.Routine);
  }
  openCommentModal(args) {
    const modalRef = this.modalService.open(CommentComponent, { size: 'xl' });
    modalRef.componentInstance.title = args.rowData.Entity.JobName;
    modalRef.componentInstance.taskID = args.rowData.Entity.ID;
    modalRef.componentInstance.clientRouter = ClientRouter.Routine;
    modalRef.result.then((result) => {
      // console.log('openCommentModal From Todolist', result);
    }, (reason) => {
    });
  }

  openEditTaskModal(args) {
    const modalRef = this.modalService.open(AddTaskModalComponent, { size: 'xl' });
    modalRef.componentInstance.title = 'Edit Routine Task';
    modalRef.componentInstance.ocid = this.ocId;
    modalRef.componentInstance.edit = this.editTask(args);
    modalRef.componentInstance.jobType = JobType.Routine;
    modalRef.result.then((result) => {
      // console.log('openEditTaskModal', result);
    }, (reason) => {
      this.parentId = 0;
    });
    this.jobtypeService.changeMessage(JobType.Routine);
  }
  openTutorialModal(args) {
    const modalRef = this.modalService.open(TutorialModalComponent, { size: 'xl' });
    modalRef.componentInstance.title = 'Add Tutorial Routine Task';
    modalRef.componentInstance.taskId = this.taskId;
    modalRef.componentInstance.ocid = this.ocId;
    modalRef.componentInstance.jobType = JobType.Routine;
    modalRef.componentInstance.jobname = args.rowInfo.rowData.Entity.JobName;
    modalRef.result.then((result) => {
      // console.log('openTutorialModal', result);
    }, (reason) => {
    });
    this.jobtypeService.changeMessage(JobType.Routine);
  }
  openEditTutorialModal(args) {
    const modalRef = this.modalService.open(TutorialModalComponent, { size: 'xl' });
    modalRef.componentInstance.title = 'Edit Tutorial Routine Task';
    modalRef.componentInstance.taskId = this.taskId;
    modalRef.componentInstance.tutorialID = args.rowInfo.rowData.Entity.ID;
    modalRef.componentInstance.jobType = JobType.Routine;
    modalRef.componentInstance.ocid = this.ocId;
    modalRef.componentInstance.jobname = args.rowInfo.rowData.Entity.JobName;
    modalRef.result.then((result) => {
      // console.log('openEditTutorialModal', result);
    }, (reason) => {
    });
    this.jobtypeService.changeMessage(JobType.Routine);
  }
  openWatchTutorialWatchModal() {
    const modalRef = this.modalService.open(WatchTutorialVideoComponent, { size: 'xl' });
    modalRef.componentInstance.src = this.srcTutorial;
    modalRef.componentInstance.name = this.tutorialName;
    modalRef.result.then((result) => {
      // console.log('openWatchTutorialWatchModal', result);
    }, (reason) => {
    });
    this.jobtypeService.changeMessage(JobType.Routine);
  }
  openWatchTutorialWatchModalByButton(data) {
    const modalRef = this.modalService.open(WatchTutorialVideoComponent, { size: 'xl' });
    modalRef.componentInstance.src = data.VideoLink;
    modalRef.componentInstance.name = data.JobName;
    modalRef.result.then((result) => {
      // console.log('openWatchTutorialWatchModal', result);
    }, (reason) => {
    });
    this.jobtypeService.changeMessage(JobType.Routine);
  }
}
