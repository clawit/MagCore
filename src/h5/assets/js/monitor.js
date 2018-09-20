//var baseUrl = 'http://api.magcore.clawit.com/';
var baseUrl = 'http://localhost:8000/';

var mapApi = baseUrl + 'api/map/';

$(function() {
    FastClick.attach(document.body);
});

$(function() {
    loadMap("rectphone");

});

var OnError = function(rr) {
    console.log('Error');
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
            //计算
            for (let i = 0; i < data.Rows.length; i++) {
                const row = data.Rows[i];
                //create ul
                var $ul=$('<ul></ul>');
                //create cells
                for (let j = 0; j < row.length; j++) {
                    const cell = row[j];
                    var $li = $('<li></li>');
                    var $img = $('<img src="../assets/images/Banana.png" />');
                    $li.append($img);
                    $ul.append($li);
                }
                //
                $('#map').append($ul);
            }
    
            //重绘
        },
        error: OnError
    });


    

}