import { MenuItem } from 'primeng/api';

export const Menu: MenuItem[] = [
  { items: [{ label: 'menu.dashboard',   icon: 'pi pi-chart-bar',    routerLink: ['/'] }] },
  { items: [{ label: 'menu.projects',    icon: 'pi pi-desktop',      routerLink: ['/projects'] }] },
  { items: [{ label: 'menu.clients',     icon: 'pi pi-users',        routerLink: ['/clients'] }] },
  { items: [{ label: 'menu.reports',     icon: 'pi pi-chart-line',   routerLink: ['/reports'] }] },
  { items: [{ label: 'menu.configuration', icon: 'pi pi-cog',       routerLink: ['/config'] }] },
];
