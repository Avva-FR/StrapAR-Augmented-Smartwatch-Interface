// Check for roatating support
var isSupport = tizen.systeminfo.getCapability('http://tizen.org/feature/input.rotating_bezel');
console.log(' Bezel = ' + isSupport);

// send rotation Information
document.addEventListener('rotarydetent', function(ev) {
    /* Get the direction value from the event */
    var direction = ev.detail.direction;
    //var textbox = document.querySelector('.contents');
    //box = document.querySelector('#textbox');

    if (direction == 'CW') {
        /* Add behavior for clockwise rotation */
    		sendMessage('cw');
    } else if (direction == 'CCW') {
        /* Add behavior for counter-clockwise rotation */
        	sendMessage('ccw');
    }
});
