var timeoutId = 0;
$('#search').on('keyup', () => {
    clearTimeout(timeoutId);
    timeoutId = setTimeout(() => {
        console.log($("#search").val());
        $.ajax({
            type: "get",
            url: "api/Pic/Search",
            data: { "input": $("#search").val() },
            //dataType: "dataType",
            success: function (response) {
                console.log(response);
                $(".picFrame").html('<div style="flex:50%"></div>');
                response.forEach(element => {
                    let pic = '<div align="center" style="margin-top:20px;flex:50%"><a>' +
                        '<img style="width:80%" class="lazy" data-original="' + element.picFileName + '"></a>' +
                        '<a><p><font color="gray" size=3>' + element.picName + '</font></p></a></div>';
                    $(".picFrame").prepend(pic);
                });
                $("img.lazy").lazyload({
                    placeholder: "/upload/201906051910162019060414205927.png",
                    effect: "fadeIn",
                    threshold: 200
                });
                console.log('finish');
            }
        });
    }, 1000);
});
