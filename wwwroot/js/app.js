window.setup = (id, config) => {
    var ctx = document.getElementById(id).getContext('2d');

    let chartStatus = Chart.getChart(id);

    if (chartStatus != undefined) {

        chartStatus.destroy();

    }

    new Chart(ctx, config);
}

(function () {
    window.QuillFunctions = {

        // Creation of Quill

        createQuill: function (quillElement, theme, toolbarModel, readonly = false) {
            if (!theme) {
                theme = "snow";
            }

            if (toolbarModel) {
                var toolbarOptions = QuillFunctions.getToolbarOptions(toolbarModel);
            }

            // Set quill at the object we can call
            // methods on later
            new Quill(quillElement, {
                modules: {
                    toolbar: toolbarOptions
                },
                readonly: readonly,
                theme: theme,
                placeholder: 'Once upon a time...'
            });
        },

        
        getQuillContent: function (quillControl) {
            return JSON.stringify(quillControl.__quill.getContents());
        },

        getQuillText: function (quillControl) {
            return quillControl.__quill.getText();
        },

        getQuillHTML: function (quillControl) {
            return quillControl.__quill.root.innerHTML;
        },

        loadQuillContent: function (quillControl, quillContent) {
            content = JSON.parse(quillContent);
            return quillControl.__quill.setContents(content, 'api');
        },

        // Toolbar functions

        getToolbarOptions: function (toolbarModel) {

            let a = QuillFunctions.getGroupA(toolbarModel);
            let b = QuillFunctions.getGroupB(toolbarModel);
            let c = QuillFunctions.getGroupC(toolbarModel);
            let d = QuillFunctions.getGroupD(toolbarModel);
            let e = QuillFunctions.getGroupE(toolbarModel);
            let f = QuillFunctions.getGroupF(toolbarModel);
            let g = QuillFunctions.getGroupG(toolbarModel);
            let h = QuillFunctions.getGroupH(toolbarModel);
            let i = QuillFunctions.getGroupI(toolbarModel);
            let j = QuillFunctions.getGroupJ(toolbarModel);
            let k = QuillFunctions.getGroupK(toolbarModel);
            let l = QuillFunctions.getGroupL(toolbarModel);

            let toolbarOptions = [];

            QuillFunctions.addGroup(toolbarOptions, a);
            QuillFunctions.addGroup(toolbarOptions, b);
            QuillFunctions.addGroup(toolbarOptions, c);
            QuillFunctions.addGroup(toolbarOptions, d);
            QuillFunctions.addGroup(toolbarOptions, e);
            QuillFunctions.addGroup(toolbarOptions, f);
            QuillFunctions.addGroup(toolbarOptions, g);
            QuillFunctions.addGroup(toolbarOptions, h);
            QuillFunctions.addGroup(toolbarOptions, i);
            QuillFunctions.addGroup(toolbarOptions, j);
            QuillFunctions.addGroup(toolbarOptions, k);
            QuillFunctions.addGroup(toolbarOptions, l);

            return toolbarOptions;
        },

        addGroup: function (parent, child) {
            if (child.length > 0) {
                parent.push(child);
            }
        },

        getGroupA: function (toolbarModel) {
            let group = [];

            if (toolbarModel.showBold) {
                group.push('bold');
            }

            if (toolbarModel.showItalic) {
                group.push('italic');
            }

            if (toolbarModel.showUnderline) {
                group.push('underline');
            }

            if (toolbarModel.showStrike) {
                group.push('strike');
            }

            return group;
        },

        getGroupB: function (toolbarModel) {
            let group = [];

            if (toolbarModel.showBlockquote) {
                group.push('blockquote');
            }

            if (toolbarModel.showCodeblock) {
                group.push('codeblock');
            }

            return group;
        },

        getGroupC: function (toolbarModel) {
            let group = [];

            if (toolbarModel.showHeading1) {
                group.push({ 'header': 1 });
            }

            if (toolbarModel.showHeading2) {
                group.push({ 'header': 2 });
            }

            return group;
        },

        getGroupD: function (toolbarModel) {
            let group = [];

            if (toolbarModel.showUnorderedList) {
                group.push({ 'list': 'unordered' });
            }

            if (toolbarModel.showOrderedList) {
                group.push({ 'list': 'ordered' });
            }

            return group;
        },

        getGroupE: function (toolbarModel) {
            let group = [];

            if (toolbarModel.showSuperscript) {
                group.push({ 'script': 'super' });
            }

            if (toolbarModel.showSubscript) {
                group.push({ 'script': 'sub' });
            }

            return group;
        },

        getGroupF: function (toolbarModel) {
            let group = [];

            if (toolbarModel.showIndentForward) {
                group.push({ 'indent': '+1' });
            }

            if (toolbarModel.showIndentBackward) {
                group.push({ 'index': '-1' });
            }

            return group;
        },

        getGroupG: function (toolbarModel) {
            let group = [];

            if (toolbarModel.showRightToLeft) {
                group.push({ 'direction': 'rtl' });
            }

            return group;
        },

        getGroupH: function (toolbarModel) {
            let group = [];

            if (toolbarModel.showFontSizes) {
                group.push({ 'size': ['small', false, 'large', 'huge'] });
            }

            return group;
        },

        getGroupI: function (toolbarModel) {
            let group = [];

            if (toolbarModel.showFontColor) {
                group.push({ 'color': [] });
            }

            if (toolbarModel.showFontBackgroundColor) {
                group.push({ 'background': [] });
            }

            return group;
        },

        getGroupJ: function (toolbarModel) {
            let group = [];

            if (toolbarModel.showFonts) {
                group.push({ 'font': [] });
            }

            return group;
        },

        getGroupK: function (toolbarModel) {
            let group = [];

            if (toolbarModel.showAlignments) {
                group.push('clean');
            }

            return group;
        },

        getGroupL: function (toolbarModel) {
            let group = [];

            if (toolbarModel.showClean) {
                group.push('clean');
            }

            return group;
        },

    };

    window.typewriterOn = () => {
        let editors = document.getElementsByClassName('ql-editor');

        if (editors.length == 0)
            return;

        let editor = editors[0];

        editor.classList.add('is-typewriter');
    };

    window.typewriterOff = () => {
        let editors = document.getElementsByClassName('ql-editor');

        if (editors.length == 0)
            return;

        let editor = editors[0];

        editor.classList.remove('is-typewriter');
    };

    window.editorOnKeyUp = () => {
        let editors = document.getElementsByClassName('ql-editor');

        if (editors.length == 0)
            return;

        let editor = editors[0];

        editor.scrollIntoView(false);
    }

    window.scrollTop = () => {
        window.scrollTo({ top: 0, behavior: 'smooth' });
    }

    window.downloadFromUrl = (options) => {
        var _a;
        const anchorElement = document.createElement('a');
        anchorElement.href = options.url;
        anchorElement.download = (_a = options.fileName) !== null && _a !== void 0 ? _a : '';
        anchorElement.click();
        anchorElement.remove();
    }

    window.downloadFromByteArray = (options) => {

        // Convert base64 string to numbers array.
        const numArray = atob(options.byteArray).split('').map(c => c.charCodeAt(0));

        // Convert numbers array to Uint8Array object.
        const uint8Array = new Uint8Array(numArray);

        // Wrap it by Blob object.
        const blob = new Blob([uint8Array], { type: options.contentType });

        // Create "object URL" that is linked to the Blob object.
        const url = URL.createObjectURL(blob);

        // Invoke download helper function that implemented in 
        // the earlier section of this article.
        downloadFromUrl({ url: url, fileName: options.fileName });

        // At last, release unused resources.
        URL.revokeObjectURL(url);
    }


})();