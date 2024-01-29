
document.getElementById('meetingBefore').addEventListener('click', function () {
    document.getElementById('meetingBefore').style.display = 'none';
    document.getElementById('meetingAfter').style.display = '';
});

// When zoom-code is none, join-button is not active
function CheckCodeJoin() {
    var inputField = document.getElementById('inputCode');
    var submitButton = document.getElementById('joinBtn');
    if (inputField.value.trim() !== '') {
        submitButton.removeAttribute('disabled');
    } else {
        submitButton.setAttribute('disabled', 'true');
    }

};
