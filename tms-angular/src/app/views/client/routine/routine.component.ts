import { Component, OnInit, ViewChild, OnDestroy } from '@angular/core';
import { RoutineService } from 'src/app/_core/_service/routine.service';
import { ActivatedRoute } from '@angular/router';
import { AlertifyService } from 'src/app/_core/_service/alertify.service';
import { FilterSettingsModel, PageService, TreeGridComponent } from '@syncfusion/ej2-angular-treegrid';
import {
  NgbModalConfig,
  NgbModal,
  NgbModalRef
} from '@ng-bootstrap/ng-bootstrap';
import { AddTaskService } from 'src/app/_core/_service/addTask.service';
import { ProjectDetailService } from 'src/app/_core/_service/projectDetail.service';
import { MenuEventArgs } from '@syncfusion/ej2-angular-navigations';
import { AddTaskModalComponent } from './add-task-modal/add-task-modal.component';
import { Task } from 'src/app/_core/_model/Task';
import { NgxSpinnerService } from 'ngx-spinner';
import { getValue, isNullOrUndefined } from '@syncfusion/ej2-base';
import { BeforeOpenCloseEventArgs } from '@syncfusion/ej2-angular-inputs';
import { TutorialModalComponent } from './tutorial-modal/tutorial-modal.component';
import { WatchTutorialVideoComponent } from './watch-tutorial-video/watch-tutorial-video.component';
import { JobType, PeriodType } from 'src/app/_core/enum/task.enum';
import { JobTypeService } from 'src/app/_core/_service/jobType.service';
import { CommentComponent } from '../modals/comment/comment.component';
import { ClientRouter } from 'src/app/_core/enum/ClientRouter';
import { RoutineDetailComponent } from '../modals/routine-detail/routine-detail.component';
import { AddTask } from 'src/app/_core/_model/add.task';
import { Subscription } from 'rxjs';
declare let $: any;
@Component({
  selector: 'app-routine',
  templateUrl: './routine.component.html',
  styleUrls: ['./routine.component.css'],
  providers: [PageService, NgbModal]
})
export class RoutineComponent implements OnInit, OnDestroy {
  @ViewChild('treegridTasks')
  public treeGridObj: TreeGridComponent;
  public ocs: object;
  public srcTutorial: string;
  public taskId: number;
  public showTasks: boolean;
  public parentId: number;
  public toolbarOptions: any[];
  public toolbarOptionsTasks: any[];
  public tasks: object;
  public pageSetting: object;
  public listOCs = JSON.parse(localStorage.getItem('user')).User.ListOCs;
  public ocLevel = JSON.parse(localStorage.getItem('user')).User.OCLevel;
  public isLeader = JSON.parse(localStorage.getItem('user')).User.IsLeader;
  public currentUser = JSON.parse(localStorage.getItem('user')).User.ID;
  public contextMenuItems: object;
  public filterSettings: FilterSettingsModel;
  searchSettings: object;
  ocId = 0;
  jobName: string;
  subscriptions: Subscription = new Subscription();
  constructor(
    config: NgbModalConfig,
    private modalService: NgbModal,
    private spinner: NgxSpinnerService,
    private addTaskService: AddTaskService,
    private jobtypeService: JobTypeService,
    private projectDetailService: ProjectDetailService,
    private routineService: RoutineService,
    private alertify: AlertifyService,
    private route: ActivatedRoute) { }

  ngOnInit() {
    this.resolver();
    this.optionGridTree();
    this.checkRole();
    this.subscriptions.add(this.addTaskService.currentMessage.subscribe((res: AddTask) => {
      if (res.Jobtype === JobType.Routine) {
        this.ocId = res.OcId;
        this.getTasks();
      }
    }));
  }
  ngOnDestroy() {
    this.subscriptions.unsubscribe();
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
  resolver() {
    this.route.data.subscribe(data => {
      this.ocs = data.ocs;
      this.onRouteChange();
    });
  }
  onRouteChange() {
    this.route.data.subscribe(data => {
      const taskname = this.route.snapshot.paramMap.get('taskname');
      this.searchSettings = {
        hierarchyMode: 'Parent',
        fields: ['Entity.JobName'],
        operator: 'contains',
        key: taskname || '',
        ignoreCase: true
      };
    });
  }
  getOCs() {
    this.routineService.getOCs().subscribe(res => this.ocs = res);
  }
  getTasks() {
    if (this.ocId) {
        $('#overlay').fadeIn();
        this.routineService.getTasks(this.ocId).subscribe(res => {
         $('#overlay').fadeOut();
         this.tasks = res;
      });
    } else {
      this.alertify.message('Please select on OC!!!');
    }
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
                this.getTasks();
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
  deleteRoot() {
    if (this.jobName) {
      this.alertify.confirm(
        'Delete Routine Root Task',
        'Are you sure you want to delete this Job Name "' + this.jobName + '" ?',
        () => {
          this.routineService.deleteRoot(this.jobName).subscribe(
            (res: any) => {
              if (res.status === -1) {
                this.alertify.warning(res.message, true);
              }
              if (res.status === 1) {
                this.alertify.success(res.message);
                this.getTasks();
              }
              if (res.status === 0) {
                this.alertify.warning(res.message, true);
              }
            },
            error => {
              this.alertify.error('Failed to delete the Job Name');
            }
          );
        }
      );
    }
  }
  checkRole() {
    if (this.ocLevel >= 3 && !this.isLeader) {
      this.toolbarOptionsTasks = [
        'Search',
        'ExpandAll',
        'CollapseAll',
        'ExcelExport',
        'Print',
      ];
      this.contextMenuItems = [
      ];
    } else {
      this.toolbarOptionsTasks = [
        { text: 'Add New', tooltipText: 'Add New', prefixIcon: 'e-add', id: 'CreateNew' },
        'Search',
        'ExpandAll',
        'CollapseAll',
        'ExcelExport',
        'Print',
      ];
      this.contextMenuItems = [
        {
          text: 'Delete',
          iconCss: ' e-icons e-delete',
          target: '.e-content',
          id: 'DeleteTask'
        },
      ];
    }
  }
  // event
  toolbarClick(args: any): void {
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
    }
  }

  contextMenuOpen(arg?: any): void {
  }
  contextMenuClick(args?: any): void {
    console.log('contextMenuClick', args);
    const data = args.rowInfo.rowData;
    // console.log('contextMenuClickdata', data);

    // this.taskId = data.ID;
    this.jobName = data.JobName;
    this.srcTutorial = data.VideoLink;
    switch (args.item.id) {
      case 'DeleteTask':
        this.deleteRoot();
        break;
    }
  }
  rowSelected(args?) {
    this.ocId = args.data.key;
    if (this.listOCs.includes(this.ocId)) {
      this.showTasks = true;
    } else {
      this.showTasks = false;
      this.alertify.validation('Warning!', 'You don\'t belong to this department');
    }
    this.getTasks();
  }
  // end event
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
}
