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

require('./models/Question');
var QA = mongoose.model('questions');

var players = [];
var questionList = []

QA.find({}).then(function(data){
  questionList = data;
})



io.on('connection', function(socket){
  
  console.log("Incomming Connection...");

  socket.emit('handshake');
  
  //console.log(questionList);

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

          var shuffleList = {
            list: [],
            on: 1
          }

          
          //console.log(shuffleList.list);

          players[socket.id] = shuffleList;
          players[socket.id].list = shuffle(questionList.slice())

          //console.log(players[socket.id]);

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

  socket.on('roundStart', function(data){
    var question = questionList[0];
    socket.in(data.roomCode).emit('roundStart', question);
  });

  socket.on('nextQ', function(data){
    //hostDict[data.roomCode].emit("ding"); //answer a question get a ding
    console.log("serving question");
    var question = players[socket.id].list[players[socket.id].on++];
    socket.emit('receiveQ', question);
  });

  socket.on('roundEnd', function(data){
    socket.in(data.roomCode).emit('TimeUp');
  })

  socket.on('sendScore', function(data){
    console.log(data.userName + ": " + data.playerScore);
    hostDict[data.roomCode].emit("score", data);
  })

  function getRandomInt(max) {
    return Math.floor(Math.random() * Math.floor(max));
  }

  function shuffle(arr){
    var tmplst = [];
    tmplst.push(arr[0]);
      arr.splice(0, 1);
    for(var i = 0; i < arr.length; i++){
      var rmdnbr = getRandomInt(arr.length);
      tmplst.push(arr[rmdnbr]);
      arr.splice(rmdnbr, 1);
    }
    return tmplst;
  }



});
