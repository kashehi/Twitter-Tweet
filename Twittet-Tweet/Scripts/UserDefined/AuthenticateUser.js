$(document).ready(function () {
   CheckState();
   });
function CheckState() {
    Ajax("CkeckState/CheckStateUser", null, SuccessCheckState, function () { }, "get");
}

function SuccessCheckState(response) {

    if (response == "") {
        TwitterAuth();
    }

}

function TwitterAuth() {
    Ajax("Authenticate/TwitterAuth", null, SuccessTwitterAuth, function () { }, "get");
}

function SuccessTwitterAuth(response) {
    window.location.replace(response);
}