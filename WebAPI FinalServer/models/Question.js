var mongoose = require('mongoose');
var Schema = mongoose.Schema;

var QuestionSchema = new Schema({
    question:{
        type:String
    },
    ansA:{
        type:String
    },
    ansB:{
        type:String
    },
    ansC:{
        type:String
    },
    ans:{
        type:Number
    }
});

mongoose.model('qna', QuestionSchema);