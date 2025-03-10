import { Route } from "@angular/router";
import { AuthPageComponent } from "./auth-page/auth-page.component";

export default [
    {
        path:'',
        component: AuthPageComponent,
        children: [
            {path: '', redirectTo:'login', pathMatch:'full'},
            {
                path:'login',
                title: 'Autenticación',
                loadComponent: ()=> import('./login/login.component')
            }
        ]
    }
] as Route[];