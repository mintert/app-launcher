var express = require('express')
var app = express()

app.set('port', (process.env.PORT || 3000))
app.use(express.static(__dirname + '/public'))

app.get('/', function(request, response) {
  response.sendFile(__dirname + '/index.html');
})

app.get('/launcher', function(request, response) {
  response.download(__dirname + '/public/AppLauncher.exe', 'AppLauncher.exe');
})

app.listen(app.get('port'), function() {
  console.log("Node app is running at http://localhost:" + app.get('port'))
})
