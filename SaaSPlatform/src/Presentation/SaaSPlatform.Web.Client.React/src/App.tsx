import { Routes, Route } from 'react-router-dom';
import { Layout } from './components/Layout';
import { RequestPage } from './pages/RequestPage';
import { StatusPage } from './pages/StatusPage';
import { ConfirmationPage } from './pages/ConfirmationPage';

function App() {
  return (
    <Layout>
      <Routes>
        <Route path="/" element={<RequestPage />} />
        <Route path="/status" element={<StatusPage />} />
        <Route path="/confirmation" element={<ConfirmationPage />} />
      </Routes>
    </Layout>
  );
}

export default App;
