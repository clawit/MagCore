
//var baseUrl = 'http://api.magcore.clawit.com/';
var baseUrl = 'http://localhost:8000/';

var mapApi = baseUrl + 'api/map/';
var gameApi = baseUrl + 'api/game/';

$(function() {
    FastClick.attach(document.body);
});

$(function() {
    $.showLoading();

    loadGame('e97b72ac913342c4abee5b4e1c285185');

});

var OnError = function(rr) {
    console.log('Error');

    $.hideLoading();
}

var loadMap = function(map) {
    //清理
    $('#map').empty();

    //获取map
    $.ajax({
        url: mapApi + map,
        type: "GET",
        dataType: 'json',
        success: function(data) {
            console.log('get map success');
            console.log(data);
            
            //计算li节点的大小
            var widthLi = $('#map').width() / data.Rows[0].length;

            //创建节点
            for (let i = 0; i < data.Rows.length; i++) {
                const row = data.Rows[i];
                //create ul
                var $ul=$('<ul></ul>');
                //create cells
                for (let j = 0; j < row.length; j++) {
                    const cell = row[j];
                    var idLi = 'li-' + j + '-' +i;
                    var $li = $('<li id="' + idLi + '"></li>');

                    var $img = $('<img />');
                    $li.append($img);
                    $ul.append($li);

                    //判断cell的类型
                    var bgImg = 'url(../assets/images/Rect.png)';
                    switch (cell) {
                        case '0':
                            bgImg = 'url(../assets/images/Empty.png)';
                            break;
                        case '1':
                            bgImg = 'url(../assets/images/Rect.png)';
                            break;
                        case '2':
                            bgImg = 'url(../assets/images/Base.png)';
                            break;
                        default:
                            break;
                    }

                    //设置li节点的css
                    $li.css({ 
                        "width": widthLi, 
                        "height": widthLi,
                        "background-size": widthLi,
                        "background-image": bgImg
                   });
                }
                //
                $('#map').append($ul);
            }
    
            //开始重绘线程
            startWorker();

            //隐藏loading
            $.hideLoading();
        },
        error: OnError
    });


    

}

var loadGame = function(gid) {
    $.ajax({
        url: gameApi + gid,
        type: "GET",
        dataType: 'json',
        success: function(data) {
            console.log('get game success');
            console.log(data);
            
            //首次加载游戏时要先加载地图
            if(databus.game == undefined){
                loadMap(data.Map);
            }

            databus.game = data;
        },
        error: OnError
    });
}