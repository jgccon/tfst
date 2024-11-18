import { Routes } from '@angular/router';
import { AppLayoutComponent } from './layout/app.layout.component';

export const routes: Routes = [
    {
        path:"",
        component: AppLayoutComponent,
    },
    {
        path:'auth',
        loadChildren: ()=> import('./features/auth/auth.routes')
    }
    
];
