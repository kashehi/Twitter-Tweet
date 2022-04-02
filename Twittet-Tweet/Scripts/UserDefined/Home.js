$(document).ready(function () {
    //to recognize user login or not
    // obtain reference to the hub proxy and hub itself
    var theHub = $.connection.hubTweet;
    // this is the function that the server will call to broadcast new tweets
    theHub.client.broadcast = function (tweet) {
        var data = (JSON.stringify(tweet));
        var obj = jQuery.parseJSON(data);
        let i = obj.Id;
        $(".test.one").insertAfter(".test.three");
        $('.tweet-wrapper').prepend($('<div id="tweet' + i + '">').load('../Forms/Tweet.html', function () {
            $('#tweet-wrap').attr('id', 'tweet-wrap' + i);
            $("#createdid").attr('id', 'createdid' + i).text(obj.CreatedBy.Name);
            $("#createddate").attr('id', 'createddate' + i).text(obj.CreatedAt);
            $("#fulltext").attr('id', 'fulltext' + i).text(obj.FullText);
            $("#avatar").attr('id', 'avatar' + i).attr("src", obj.CreatedBy.ProfileImageUrl);
            $("#retweet-count").attr('id', 'retweet-count' + i).text(obj.RetweetCount);
            $("#likes-count").attr('id', 'likes-count' + i).text(obj.FavoriteCount);
            $("#comment-count").attr('id', 'comment-count' + i).text(obj.QuoteCount);
        
            $("#media").attr('id', 'Media' + i).attr("src", obj.Media[0].media_url);

        }));


    };

    // this is a function that indicates that connection to the hub has been successful
    $.connection.hub.start().done(function () {
        theHub.server.getTweets();
    });

});





