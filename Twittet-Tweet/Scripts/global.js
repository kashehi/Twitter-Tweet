function Ajax(url, data, onDone, onFail, type) {
    type = type || "post";

    if (type.toLowerCase().trim() == "post") {
        data = JSON.stringify(data);
    }

    $.ajax({
        type: type,
        url: "/api/" + url,
        data: data,
        cache: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json "
    })
        .done(onDone)
        .fail(onFail);
}