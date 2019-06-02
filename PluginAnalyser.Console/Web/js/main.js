(function () {
    function FileDragHover(e) {
        e.stopPropagation();
        e.preventDefault();
        e.target.className = (e.type == "dragover" ? "hover" : "");
    };

    function FileSelectHandler(e) {
        FileDragHover(e);
        var files = e.target.files || e.dataTransfer.files;

        document.getElementById("messages").innerHTML = '';
        for (var i = 0, f; f = files[i]; i++) {
            ProcessFile(f);
        }
    };

    function ProcessFile(file) {
        var child = document.createElement('div');
        child.innerHTML =
            'File: <strong>' + file.name + '</strong> ' +
            'type: <strong>' + file.type + '</strong> ' +
            'size: <strong>' + file.size + '</strong> bytes';

        //var canvas = document.createElement('canvas');
        //child.appendChild(canvas);
        //child.throbber = canvas;

        document.getElementById("messages").appendChild(child);
    };

    var fileselect = document.getElementById("fileselect"),
        filedrag = document.getElementById("filedrag");

    fileselect.addEventListener("change", FileSelectHandler, false);

    if (filedrag) {
        filedrag.addEventListener("dragover", FileDragHover, false);
        filedrag.addEventListener("dragleave", FileDragHover, false);
        filedrag.addEventListener("drop", FileSelectHandler, false);
        filedrag.style.display = "block";
    }

    var bar = new ProgressBar.Circle('#progress', {
        color: '#aaa',
        // This has to be the same size as the maximum width to
        // prevent clipping
        strokeWidth: 10,
        trailWidth: 1,
        easing: 'bounce',
        duration: 1400,
        text: {
            autoStyleContainer: false
        },
        from: { color: '#aaa', width: 5 },
        to: { color: '#333', width: 10 },
        // Set default step function for all animate calls
        step: function (state, circle) {
            circle.path.setAttribute('stroke', state.color);
            circle.path.setAttribute('stroke-width', state.width);

            var value = Math.round(circle.value() * 100);
            if (value === 0) {
                circle.setText('');
            } else {
                circle.setText(value);
            }

        }
    });
    bar.text.style.fontFamily = '"Raleway", Helvetica, sans-serif';
    bar.text.style.fontSize = '15px';

    $('#submitbutton').click(function (evt) {
        evt.preventDefault();

        var formData = new FormData(document.getElementById('FormUpload'));

        function onPostProgress(evt) {
            if (evt.lengthComputable) {
                var percentComplete = evt.loaded / evt.total;
                bar.animate(percentComplete);  // Number from 0.0 to 1.0
            }
        };

        $.ajax({
            url: '/analyse/?output=html',
            type: 'POST',

            data: formData,
            cache: false,
            contentType: false,
            processData: false,

            xhr: function () {
                var xhr = new window.XMLHttpRequest();
                xhr.upload.addEventListener("progress", onPostProgress, false);
                xhr.addEventListener("progress", onPostProgress, false);

                return xhr;
            },

            success: function (res) {
                var results = document.getElementById('results');
                var iframe = window.analyserIframe;
                if (!iframe) {
                    iframe = document.createElement('iframe');
                    window.analyserIframe = iframe;
                }
                iframe.src = 'data:text/html;charset=utf-8,' + encodeURIComponent(res);
                results.appendChild(iframe);

                bar.set(0);
            }
        });
    });
})();
