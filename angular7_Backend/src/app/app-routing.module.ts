import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { IndexComponent } from './views/index/index.component';
import { AddPostDialogComponent} from './views/admin/add-post-dialog/add-post-dialog.component'
import { MyNewComponentComponent } from './views/my-new-component/my-new-component.component';


const routes: Routes = [
  { path: 'index', component: IndexComponent },
  { path: 'admin', loadChildren: './views/admin/admin.module#AdminModule' },
  { path: 'my-new-component', component:MyNewComponentComponent },
  { path: '', redirectTo: 'index', pathMatch: 'full'  },
  { path: '**', redirectTo: 'index', pathMatch: 'full' }

];

@NgModule({
  imports: [RouterModule.forRoot(routes, { useHash: true })],
  exports: [RouterModule]
})
export class AppRoutingModule { }

export const routingComponents= [AddPostDialogComponent]
