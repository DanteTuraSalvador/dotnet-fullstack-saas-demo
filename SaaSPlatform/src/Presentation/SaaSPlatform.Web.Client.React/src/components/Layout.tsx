import type { PropsWithChildren } from 'react';
import { NavLink } from 'react-router-dom';

export function Layout({ children }: PropsWithChildren) {
  const navClass = ({ isActive }: { isActive: boolean }) =>
    [
      'text-sm font-medium transition hover:text-primary',
      isActive ? 'text-primary font-semibold' : 'text-slate-500'
    ].join(' ');

  return (
    <div className="min-h-screen bg-slate-50 text-slate-900">
      <div className="flex min-h-screen flex-col">
        <header className="border-b border-slate-100 bg-white/90 backdrop-blur">
          <div className="page-shell flex h-16 items-center justify-between">
            <NavLink to="/" className="text-lg font-semibold text-slate-900">
              SaaSPlatform React
            </NavLink>
            <nav className="flex items-center gap-4">
              <NavLink to="/" className={navClass} end>
                Request Subscription
              </NavLink>
              <NavLink to="/status" className={navClass}>
                Check Status
              </NavLink>
            </nav>
          </div>
        </header>

        <main className="page-shell flex-1 py-10">{children}</main>

        <footer className="border-t border-slate-100 bg-white/70 py-6 text-sm text-slate-500">
          <div className="page-shell flex flex-wrap items-center justify-between gap-2">
            <span>&copy; {new Date().getFullYear()} SaaSPlatform</span>
            <span className="font-medium text-slate-600">React Client</span>
          </div>
        </footer>
      </div>
    </div>
  );
}
