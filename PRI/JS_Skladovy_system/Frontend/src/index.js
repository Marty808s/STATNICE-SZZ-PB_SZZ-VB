import React from 'react';
import ReactDOM from 'react-dom/client';
import './index.css';
import LoginPage from '@pages/LoginPage';
import ProduktyPage from './pages/ProduktyPage';
import reportWebVitals from './reportWebVitals';
import { BrowserRouter, Route, Routes } from 'react-router-dom';
import UserProvider from '@hooks/UserProvider';
import { MessageProvider } from './hooks/MessageContext';
import MessageToast from '@components/MessageBox/MessageToast';

const root = ReactDOM.createRoot(document.getElementById('root'));
root.render(
  <React.StrictMode>
    <MessageProvider>
      <UserProvider>
        <BrowserRouter>
          <Routes>
            <Route path='/' element={<LoginPage />}/>
            <Route path='/produkty' element={<ProduktyPage />}/>
          </Routes>
        </BrowserRouter>
        <MessageToast />
      </UserProvider>
    </MessageProvider>
  </React.StrictMode>
);

reportWebVitals();
