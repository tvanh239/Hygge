var avaUser = document.getElementById("avatarImg");
var settingMenu = document.getElementById("settingMenu");

///---------------------Ready-------------------------
document.addEventListener('DOMContentLoaded', function () {
    // Your code here, executed when the document is ready
    console.log('Document is ready!');
    // Check for browser compatibility
    navigator.getUserMedia = navigator.getUserMedia || navigator.webkitGetUserMedia || navigator.mozGetUserMedia || navigator.msGetUserMedia;

    if (navigator.getUserMedia) {
        // Request microphone access
        navigator.getUserMedia({ audio: true }, function (stream) {
            // Get the microphone track
            const audioTracks = stream.getAudioTracks();
            if (audioTracks.length > 0) {
                // Get the select element
                var selectElement = document.getElementById('micSetting');

                // Populate options based on the data
                audioTracks.forEach(function (option) {
                    var optionElement = document.createElement('option');
                    optionElement.value = option.label;
                    optionElement.text = option.label;
                    selectElement.add(optionElement);
                });
            } else {
                console.log('No microphone');
           
            }
        }, function (error) {
            console.error('Error accessing microphone:', error);
            document.getElementById('microphoneName').innerText = 'Error accessing microphone';
        });
    }
});
///-----------------------------------------------------

///----------------------------- Dialog for user avatar　↓----------------------------

avaUser.addEventListener("click", function () {
    var dialogInfo = document.getElementById("personalInfo");
    dialogInfo.showModal();
});

document.getElementById("closeInfo").addEventListener("click", function () {
    var dialogInfo = document.getElementById("personalInfo");
    dialogInfo.close();
});

function handleClickOutside(event) {
    var dialogInfo = document.getElementById("personalInfo");
    if (!dialogInfo.contains(event.target) && event.target != document.getElementsByClassName("image-avatar")[0]) {
        // Click is outside the dialog, so close it
        dialogInfo.close();
    }
    var menuDialog = document.getElementById("menuIcon");
    if (!menuDialog.contains(event.target) && event.target != document.getElementsByClassName("btn-setting")[0]) {
        // Click is outside the dialog, so close it
        settingMenu.classList.remove('card-active') ;
        menuDialog.close();
    }
}

///-------------------------------Dialog for user avatar　↑------------------------------



///----------------------------- Dialog for menu icon ↓--------------------------------
settingMenu.addEventListener("click", function () {
    this.className = this.className + ' card-active';
    var dialogInfo = document.getElementById("menuIcon");
    CreateMenuIcons();
    dialogInfo.showModal();
    
});
function CreateMenuIcons() {

}

document.addEventListener('click', handleClickOutside);
///------------------------------Dialog for menu icon ↑--------------------------------