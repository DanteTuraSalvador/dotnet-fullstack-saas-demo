import type { PropsWithChildren } from 'react';
import { NavLink } from 'react-router-dom';

export function Layout({ children }: PropsWithChildren) {
  return (
    <div className="d-flex flex-column min-vh-100">
      <header className="border-bottom bg-white shadow-sm">
        <nav className="navbar navbar-expand-lg navbar-light container">
          <NavLink className="navbar-brand fw-semibold" to="/">
            SaaSPlatform React
          </NavLink>
          <button
            className="navbar-toggler"
            type="button"
            data-bs-toggle="collapse"
            data-bs-target="#mainNav"
            aria-controls="mainNav"
            aria-expanded="false"
            aria-label="Toggle navigation"
          >
            <span className="navbar-toggler-icon"></span>
          </button>
          <div className="collapse navbar-collapse" id="mainNav">
            <ul className="navbar-nav ms-auto mb-2 mb-lg-0">
              <li className="nav-item">
                <NavLink to="/" className="nav-link" end>
                  Request Subscription
                </NavLink>
              </li>
              <li className="nav-item">
                <NavLink to="/status" className="nav-link">
                  Check Status
                </NavLink>
              </li>
            </ul>
          </div>
        </nav>
      </header>

      <main className="container py-4 flex-grow-1">{children}</main>

      <footer className="border-top py-3 text-muted bg-white">
        <div className="container d-flex justify-content-between small flex-wrap gap-2">
          <span>&copy; {new Date().getFullYear()} SaaSPlatform</span>
          <span>React Client</span>
        </div>
      </footer>
    </div>
  );
}
