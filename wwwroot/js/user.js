var avaUser = document.getElementById("avatarImg");
var settingMenu = document.getElementById("settingMenu");

///---------------------Ready-------------------------
document.addEventListener('DOMContentLoaded', function () {
    if (navigator.mediaDevices.getUserMedia) {
        console.log('getUserMedia supported.');

        const constraints = { audio: true };
        let chunks = [];

        let onSuccess = function (stream) {

            if (!navigator.mediaDevices || !navigator.mediaDevices.enumerateDevices) {
                console.log("enumerateDevices() not supported.");
                return false;
            }
            //List microphones.
            navigator.mediaDevices.enumerateDevices().then(function (devices) {
                devices.forEach(function (device) {
                    micSetting = document.getElementById("micSetting");
                    speakSelect = document.getElementById("audioSetting");
                    videoSelect = document.getElementById("videoSetting");
                    if (device.kind === "audioinput" && device.label != "") {
                        var optionElement = document.createElement('option');
                        optionElement.value = removeLastParentheses(device.label);
                        optionElement.text = removeLastParentheses(device.label);
                        micSetting.add(optionElement);
                    } else if (device.kind === "audiooutput" && device.label != "") {
                        var optionElement = document.createElement('option');
                        optionElement.value = removeLastParentheses(device.label);
                        optionElement.text = removeLastParentheses(device.label);
                        speakSelect.add(optionElement);
                    } else if ( device.label != "") {
                        console.log(device)
                        var optionElement = document.createElement('option');
                        optionElement.value = removeLastParentheses(device.label);
                        optionElement.text = removeLastParentheses(device.label);
                        videoSelect.add(optionElement);
                    }

                });
            }).catch(function (err) {
                console.log(err);
            });

        }

        let onError = function (err) {
            console.log('The following error occured: ' + err);
        }

        navigator.mediaDevices.getUserMedia(constraints).then(onSuccess, onError);

    } else {
        console.log('getUserMedia not supported on your browser!');
    }
});
///-----------------------------------------------------


// Function to remove the last set of parentheses and its content
function removeLastParentheses(inputString) {
    return inputString.replace(/\([^)]*\)$/, '').trim();
}


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