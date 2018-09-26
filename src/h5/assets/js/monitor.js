
$(function() {
    FastClick.attach(document.body);
});

$(function() {
    //获取查询字符串,读取gid
    var queries = {};
    $.each(document.location.search.substr(1).split('&'),function(c,q){
        var i = q.split('=');
        queries[i[0]] = i[1];
    });
    console.log(queries);

    $.showLoading();

    loadGame(queries.gid);

});

var OnError = function(rr) {
    console.log('Error');

    $.hideLoading();
}

var loadMap = function(game) {
    let map = game.Map;
    //清理
    $('#map').empty();

    //获取map
    $.ajax({
        url: databus.mapApi + map,
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
                    let idLi = 'li-' + j + '-' +i;
                    let idImg = 'img-' + j + '-' +i;
                    let $li = $('<li id="' + idLi + '"></li>');

                    let $img = $('<img id="' + idImg + '" />');
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
            if(databus.inited == false){
                startWorker(game);
                databus.inited = true;
            }

            //隐藏loading
            $.hideLoading();
        },
        error: OnError
    });


    

}

var loadGame = function(gid) {
    $.ajax({
        url: databus.gameApi + gid,
        type: "GET",
        dataType: 'json',
        success: function(data) {
            console.log('get game success');
            console.log(data);

            if(data.Map == undefined){
                $.hideLoading();
                $.toast("无法定位游戏", 'cancel');
                return;
            }

            databus.game = data;

            //首次加载游戏时要先加载地图
            if(databus.inited == false){
                loadMap(data);
            }

        },
        error: OnError
    });
}

var update = function(){
    let game = databus.game;
    if(game.State != undefined && game.State == 0) {
        //更新players信息
        databus.players = new Array();
        for (var i = 0; i < game.Players.length; i++){
          var p = game.Players[i];
          databus.players[p.Index] = p;
        }
    }
}

var render = function(){
    let game = databus.game;
    if(game.State != undefined && game.State < 3) {
        for (let i = 0; i < game.Cells.length; i++) {
            const row = game.Cells[i];
            
            for (let j = 0; j < row.length; j++) {
                const cell = row[j];
                
                let idLi = 'li-' + j + '-' +i;
                let idImg = 'img-' + j + '-' +i;
                let $li = $('#' + idLi);
                let $img = $('#' + idImg);

                //设置基地
                if(cell.Type == 2 && $li != undefined ) {
                    $li.css({ 
                        "background-image": "url(../assets/images/Base.png)"
                   });
                }

                //设置玩家占领
                if(cell.State > 0 && $img != undefined) {
                    let owner = cell.Owner;
                    let player = databus.players[owner];
                    let color = player.Color;
                    let icon = databus.icons[color];
                    $img.attr('src', icon) 
                }
            }
        }
    }
    
}