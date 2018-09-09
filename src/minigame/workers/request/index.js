/*
  worker msg 格式
  {
    "action": "detail",
    "gid": "1A2ECB678CBDDDF90DFB1A2ECB678CBDDD"
    "data": {}
  }
 */


const utils = require('./utils')

worker.onMessage(function(msg){
  console.log('worker msg received:');
  console.log(msg);
  onProcess(msg);
})

function onProcess(msg){


}