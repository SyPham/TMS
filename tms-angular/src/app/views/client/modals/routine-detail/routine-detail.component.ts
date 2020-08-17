import { Component, OnInit, Input, ViewChild } from '@angular/core';
import { TreeGridComponent, FilterSettingsModel } from '@syncfusion/ej2-angular-treegrid';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { PeriodType, JobType } from 'src/app/_core/enum/task.enum';
import { NgxSpinnerService } from 'ngx-spinner';
import { AddTaskService } from 'src/app/_core/_service/addTask.service';
import { JobTypeService } from 'src/app/_core/_service/jobType.service';
import { WatchTutorialVideoComponent } from '../../routine/watch-tutorial-video/watch-tutorial-video.component';
import { RoutineService } from 'src/app/_core/_service/routine.service';
import { AlertifyService } from 'src/app/_core/_service/alertify.service';
import { CommentComponent } from '../comment/comment.component';
import { ClientRouter } from 'src/app/_core/enum/ClientRouter';
import { TutorialModalComponent } from '../../routine/tutorial-modal/tutorial-modal.component';
import { AddTaskModalComponent } from '../../routine/add-task-modal/add-task-modal.component';
import { ProjectDetailService } from 'src/app/_core/_service/projectDetail.service';
import { Task } from 'src/app/_core/_model/Task';

@Component({
  selector: 'app-routine-detail',
  templateUrl: './routine-detail.component.html',
  styleUrls: ['./routine-detail.component.css']
})
export class RoutineDetailComponent implements OnInit {
  @Input() title: string;
  @Input() tasks: any;
  public data: any;
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
  public contextMenuItems: object;
  public filterSettings: FilterSettingsModel;
  private tutorialName: string;
  searchSettings: object;
  ocId: any;
  constructor(
    private modalService: NgbModal,
    private spinner: NgxSpinnerService,
    private addTaskService: AddTaskService,
    private jobtypeService: JobTypeService,
    private routineService: RoutineService,
    private projectDetailService: ProjectDetailService,
    private alertify: AlertifyService,
    public activeModal: NgbActiveModal,
  ) { }

  ngOnInit(): void {
    this.data = this.tasks.Tasks;
    this.optionGridTree();
    console.log(this.tasks.Tasks)
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
  recordDoubleClick(agrs?: any) {
    this.openCommentModal(agrs);
  }

  contextMenuClick(args) {
    // console.log('contextMenuClick', args);
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
  toolbarClick(args) { }
  dataBound(args) { }
  periodText(enumVal) {
    return this.getEnumKeyByEnumValue(PeriodType, Number(enumVal));
  }
  getEnumKeyByEnumValue(myEnum, enumValue) {
    let keys = Object.keys(myEnum).filter(x => myEnum[x] === enumValue);
    return keys.length > 0 ? keys[0] : null;
  }

  contextMenuOpen(args) { }
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
