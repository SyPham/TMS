import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class JobTypeService {
  messageSource = new BehaviorSubject<number>(0);
  jobType = new BehaviorSubject<any>(null);
  currentMessage = this.messageSource.asObservable();
  constructor() { }
  // method này để change source message
  changeMessage(message) {
    this.messageSource.next(message);
  }
  getJobType(): Observable<any> {
    return this.jobType.asObservable();
  }

  setJobType(jobType): void {
    this.jobType.next(jobType);
  }

}
