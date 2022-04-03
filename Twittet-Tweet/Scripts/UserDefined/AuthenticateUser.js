$(document).ready(function () {
    TwitterAuth();
});

function TwitterAuth() {
    Ajax("Authenticate/TwitterAuth", null, SuccessTwitterAuth, function () { }, "get");
}

function SuccessTwitterAuth(response) {
    window.location.replace(response);
}