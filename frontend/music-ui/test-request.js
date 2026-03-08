const http = require('http');
const req = http.request({
  hostname: 'localhost',
  port: 5025,
  path: '/api/sessions',
  method: 'POST',
  headers: {
    'Content-Type': 'application/json',
    'X-Anonymous-Id': '12345678-1234-1234-1234-123456789012'
  }
}, res => {
  let data = '';
  res.on('data', chunk => data += chunk);
  res.on('end', () => console.log('Status:', res.statusCode, 'Body:', data));
});
req.write(JSON.stringify({ energy: 0.5, familiarity: 0.5, time: 0.5 }));
req.end();
