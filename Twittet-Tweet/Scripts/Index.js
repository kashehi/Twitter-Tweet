var obj;
$(document).ready(function () {
    $(function () {
        debugger;
        // obtain reference to the hub proxy and hub itself
        var theHub = $.connection.hubTweet;
        // this is the function that the server will call to broadcast new tweets
        theHub.client.broadcast = function (tweet) {
            var data = (JSON.stringify(tweet));
            var obj = data.split(";");
            var item = '<li>' + obj[0] + '</li>';
            $('#tweets').prepend(item);
            var item = '<li>' + obj[1] + '</li>';
            $('#tweets').prepend(item);
            var item = '<li>' + obj[2] + '</li>';
            $('#tweets').prepend(item);
        };
        // this is a function that indicates that connection to the hub has been successful
        $.connection.hub.start().done(function () {
            theHub.server.GetTweets();

        });

    });

});
