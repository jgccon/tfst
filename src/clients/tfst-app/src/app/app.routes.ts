import { Routes } from '@angular/router';
import { AppLayout } from './layout/component/app.layout';

export const routes: Routes = [
    {
        path: '',
        component: AppLayout,
        children: [
            {
                path: '',
                redirectTo: 'dashboard',
                pathMatch: 'full'
            },
            {
                path: 'dashboard',
                loadComponent: () => import('./features/dashboard/pages/dashboard/dashboard.component').then(
                    (m) => m.DashboardComponent
                )
            }
        ]
    },
];
