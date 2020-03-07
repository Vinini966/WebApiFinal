var createError = require('http-errors');
var express = require('express');
var exphbs = require('express-handlebars');
var bodyparser = require('body-parser');

var mongoose = require('mongoose');
var db = require('./helper/database');

mongoose.connect(db.mongoURI ,{
  useNewUrlParser:true,
  useUnifiedTopology:true
}).then(function(){
  console.log('mongodb connected');
}).catch(function(err){
  console.log(err);
});


var app = express();

app.io = io;

// view engine setup
app.engine('handlebars', exphbs({defaultLayout:'main'}));
app.set('view engine', 'handlebars');

app.use(bodyparser.json());
app.use(bodyparser.urlencoded({ extended: false }));

app.use(express.static('public'));

var hostDict = {};
var clientDict = {};

// catch 404 and forward to error handler
app.get('/', function(req, res){
  res.render('index');
});

app.post('/game', function(req, res){
  var userInfo = {
    userName: req.body.user,
    roomCode: req.body.RmCd.toUpperCase(),
    client: true
  }
  var data = JSON.stringify(userInfo)
  res.render('game', {
    clientInfo:encodeURI(data)
  });
});

var server = app.listen(3000, function(){
  console.log("Server started on port 3000")
});


var io = require('socket.io').listen(server);
var shortid = require("shortid");

var players = [];

io.on('connection', function(socket){
  
  console.log("Incomming Connection...");

  socket.emit('handshake');

  socket.on('connInfo', function(data){
    var room = io.sockets.adapter.rooms[data.roomCode];
    //console.log(room);
    if(data.client){
      console.log("Client Joined")
      if(room!= undefined){//host has created room
        if(room.length < 10){
          socket.join(data.roomCode);
          console.log("Client joined room " + data.roomCode)
          console.log("Sending host user name at " + hostDict[data.roomCode].id);
          var userNameData = {
            userName:data.userName
          }
          hostDict[data.roomCode].emit("clientJoin", userNameData);
          if(room.length == 2){//control "host" Client
            socket.emit("ClientStatus", {host:true});
          }
          else{
            socket.emit("ClientStatus", {host:false});
          }
        }
        else{
          var errMsg = {
            err: "Room Full!",
            errCode: 0
          }
          socket.emit("err", errMsg);
        }
      }
      else{
        var errMsg = {
          err: "No room with that code.",
          errCode: 0
        }
        socket.emit("err", errMsg);
      }
    }
    else{
      if(room != undefined){//host has created room
        console.log("What double room code " + data.roomCode)
        var errMsg = {
          err: "Room has already been created.",
          errCode: 1
        }
        socket.emit("err", errMsg);
      }
      else{
        socket.join(data.roomCode);
        hostDict[data.roomCode] = socket;
        console.log("Host joined at room code " + data.roomCode);
      }
    }
  });

  socket.on('beginGame', function(data){
    io.in(data.roomCode).emit('gameBegining');
  })





});
