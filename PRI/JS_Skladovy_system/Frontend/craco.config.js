const path = require('path');

module.exports = {
  webpack: {
    alias: {
      '@components': path.resolve(__dirname, 'src/components'),
      '@pages': path.resolve(__dirname, 'src/pages'),
      '@core': path.resolve(__dirname, 'src/components/core'),
      '@utils': path.resolve(__dirname, 'src/utils'),
      '@hooks': path.resolve(__dirname, 'src/hooks'),
      '@context': path.resolve(__dirname, 'src/context'),
      '@api': path.resolve(__dirname, 'src/api'),

    }
  },
  eslint: {
    enable: false,
  },
}; 