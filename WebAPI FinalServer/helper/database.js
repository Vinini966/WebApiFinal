if(process.env.NODE_ENV === "production"){
    module.exports ={
        //connection to cloud mongodb server 
        mongoURI:""
    }
}
else{
    module.exports ={
        mongoURI:"mongodb://localhost:27017/questions"
    }
}