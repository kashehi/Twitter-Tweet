var obj;
$(document).ready(function () {
    $(function () {
        TwitterAuth();
        // obtain reference to the hub proxy and hub itself
        var theHub = $.connection.hubTweet;
        // this is the function that the server will call to broadcast new tweets
        theHub.client.broadcast = function (tweet) {
            var data = (JSON.stringify(tweet));
            var obj = jQuery.parseJSON(data);
            //var obj = data.split(";");
            for (let i = 0; i < obj.length - 185; i++) {
                $('.tweet-wrap1').append($('<div id="tweet' + i + '">').load('../Forms/Tweet.html', function () {
                    $('#tweet-wrap').attr('id', 'tweet-wrap' + i);
                    $("#createdid").attr('id', 'createdid' + i).text(obj[i].CreatedBy.Name);
                    $("#createddate").attr('id', 'createddate' + i).text(obj[i].CreatedAt);
                    $("#fulltext").attr('id', 'fulltext' + i).text(obj[i].FullText);
                    $("#avatar").attr('id', 'avatar' + i).attr("src", obj[i].CreatedBy.ProfileImageUrl);
                    $("#retweet-count").attr('id', 'retweet-count' + i).text(obj[i].RetweetCount);
                    $("#likes-count").attr('id', 'likes-count' + i).text(obj[i].FavoriteCount);
                    $("#comment-count").attr('id', 'comment-count' + i).text(obj[i].QuoteCount);
                }));
            }
        };

        // this is a function that indicates that connection to the hub has been successful
        $.connection.hub.start().done(function () {
            theHub.server.getTweets();
        });
    });

});

function TwitterAuth() {
    Ajax("Home/TwitterAuth", null, SuccessTwitterAuth, function () { }, "get");
}

function SuccessTwitterAuth(response) {
    window.location.replace(response);
}
