// Write your JavaScript code.
var audio_context;

function consoleText(words, id, bucle) {
    var visible = true;
    var con = document.getElementById('console');
    var letterCount = 1;
    var x = 1;
    var waiting = false;
    var target = document.getElementById(id)
    target.setAttribute('style', 'color:#27ED00')
    window.setInterval(function () {

        if (letterCount === 0 && waiting === false) {
            waiting = true;
            target.innerHTML = words[0].substring(0, letterCount)
            window.setTimeout(function () {
                var usedWord = words.shift();
                if(!bucle)
                words.push(usedWord);
                x = 1;
                target.setAttribute('style', 'color:#27ED00');
                letterCount += x;
                waiting = false;
            }, 1500)
        } else if (letterCount === words[0].length + 1 && waiting === false) {
            waiting = true;
            if (words.length !== 1) {
                window.setTimeout(function () {
                    x = -1;
                    letterCount += x;
                    waiting = false;
                }, 1500)
            }
        } else if (waiting === false) {
            target.innerHTML = words[0].substring(0, letterCount)
            letterCount += x;
        }
    }, 120)
    window.setInterval(function () {
        if (visible === true) {
            con.className = 'console-underscore hidden'
            visible = false;

        } else {
            con.className = 'console-underscore'

            visible = true;
        }
    }, 400);
}


function activateRecordClient(recordButton, stopButton, callback, props) {
    navigator.mediaDevices.getUserMedia({
        audio: true
    })
        .then(function (stream) {
            stopButton.disabled = false;
            recordButton.addEventListener('click', startRecording);
            stopButton.addEventListener('click', function () { stopRecording(callback, props); }, false);

            audio_context = audio_context || new window.AudioContext;
            var input = audio_context.createMediaStreamSource(stream);
            recorder = new Recorder(input);
        });
}

function startRecording() {
    recordButton.disabled = true;
    stopButton.disabled = false;
    recorder.record();
}

function stopRecording(callback, props) {
    recordButton.disabled = false;
    stopButton.disabled = true;

    var audio = document.getElementById('audio');

    console.log('...working...');
    recorder && recorder.stop();

    recorder.exportWAV(function (blob) {
        callback(blob, props);
        if (audio) {
            audio.src = URL.createObjectURL(blob);
            audio.play();
        }
    });
    recorder.clear();
}