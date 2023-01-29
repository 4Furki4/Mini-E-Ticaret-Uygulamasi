import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HeaderComponent } from './header/header.component';
import { FooterComponent } from './footer/footer.component';
import { SidebarComponent } from './sidebar/sidebar.component';
import { RouterModule } from '@angular/router';
import { MatListModule } from '@angular/material/list';
import { MatButtonModule } from '@angular/material/button';


@NgModule({
  declarations: [
    HeaderComponent,
    FooterComponent,
    SidebarComponent
  ],
  imports: [
    CommonModule,
    RouterModule,
    MatListModule,
    MatButtonModule
  ],
  exports: [
    HeaderComponent,
    FooterComponent,
    SidebarComponent
  ]
})
export class ComponentsModule { }
