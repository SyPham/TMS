import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AbnormalComponent } from './abnormal.component';
import { AbnormalDetailComponent } from './abnormal-detail/abnormal-detail.component';

@NgModule({
  imports: [
    CommonModule
  ],
  declarations: [AbnormalComponent, AbnormalDetailComponent]
})
export class AbnormalModule { }
