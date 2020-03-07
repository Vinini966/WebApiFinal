var mongoose = require('mongoose');
var Schema = mongoose.Schema;

var QuestionSchema = new Schema({
    question:{
        type:String
    },
    a:{
        type:String
    },
    b:{
        type:String
    },
    c:{
        type:String
    },
    answer:{
        type:String
    }
});

mongoose.model('questions', QuestionSchema);