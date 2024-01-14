//*****************************************************************************
//* ALL RIGHTS RESERVED. COPYRIGHT (C) 2024 Hygge                             *
//*****************************************************************************
//* File Name    : Site.js　　                                                 *
//* Function     : The event for layout view                                  *
//* Create       : VietAnh 2023/12/28                                         *
//*****************************************************************************.


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
