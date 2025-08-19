import React from 'react';
import ReactDOM from 'react-dom/client';
import './index.css';
import IndexPage from '@pages/index';
import reportWebVitals from './reportWebVitals';

const root = ReactDOM.createRoot(document.getElementById('root'));
root.render(
  <React.StrictMode>
    <IndexPage />
  </React.StrictMode>
);

reportWebVitals();
