// Tournament Documents — Dropzone.js upload widget + uploaded file list management
(function () {
    Dropzone.autoDiscover = false;

    document.addEventListener('DOMContentLoaded', function () {
        var container = document.querySelector('[data-documents-widget]');
        if (!container) {
            return;
        }

        var tournamentId = container.getAttribute('data-tournament-id');
        var uploadUrl = container.getAttribute('data-upload-url');
        var listUrl = container.getAttribute('data-list-url');
        var deleteUrlTemplate = container.getAttribute('data-delete-url-template');
        var token = document.querySelector('input[name="__RequestVerificationToken"]')?.value;

        var dropzoneEl = container.querySelector('.tournament-dropzone');
        var listEl = container.querySelector('[data-document-list]');

        function formatSize(bytes) {
            if (!bytes && bytes !== 0) return '';
            var units = ['B', 'KB', 'MB', 'GB'];
            var i = 0;
            var size = bytes;
            while (size >= 1024 && i < units.length - 1) {
                size /= 1024;
                i++;
            }
            return size.toFixed(size < 10 && i > 0 ? 1 : 0) + ' ' + units[i];
        }

        function renderList(documents) {
            listEl.innerHTML = '';

            if (!documents || documents.length === 0) {
                var empty = document.createElement('p');
                empty.className = 'document-list__empty';
                empty.textContent = 'No documents uploaded yet.';
                listEl.appendChild(empty);
                return;
            }

            documents.forEach(function (doc) {
                var item = document.createElement('li');
                item.className = 'document-list__item';

                var info = document.createElement('div');
                info.className = 'document-list__info';

                var link = document.createElement('a');
                link.className = 'document-list__name';
                link.href = doc.filePath;
                link.target = '_blank';
                link.rel = 'noopener noreferrer';
                link.textContent = doc.fileName;

                var meta = document.createElement('span');
                meta.className = 'document-list__meta';
                meta.textContent = formatSize(doc.fileSize);

                info.appendChild(link);
                info.appendChild(meta);

                var deleteBtn = document.createElement('button');
                deleteBtn.type = 'button';
                deleteBtn.className = 'document-list__delete';
                deleteBtn.setAttribute('aria-label', 'Delete ' + doc.fileName);
                deleteBtn.textContent = 'Delete';
                deleteBtn.addEventListener('click', function () {
                    deleteDocument(doc.id);
                });

                item.appendChild(info);
                item.appendChild(deleteBtn);
                listEl.appendChild(item);
            });
        }

        function loadDocuments() {
            fetch(listUrl, { headers: { 'Accept': 'application/json' } })
                .then(function (res) {
                    if (!res.ok) throw new Error('Failed to load documents');
                    return res.json();
                })
                .then(renderList)
                .catch(function () {
                    listEl.innerHTML = '<p class="document-list__empty">Could not load documents.</p>';
                });
        }

        function deleteDocument(documentId) {
            var url = deleteUrlTemplate.replace('0', documentId);

            fetch(url, {
                method: 'POST',
                headers: {
                    'RequestVerificationToken': token
                }
            })
                .then(function (res) {
                    if (!res.ok) throw new Error('Failed to delete document');
                    loadDocuments();
                })
                .catch(function () {
                    alert('Could not delete the document. Please try again.');
                });
        }

        if (dropzoneEl) {
            new Dropzone(dropzoneEl, {
                url: uploadUrl,
                paramName: 'file',
                maxFilesize: 25,
                addRemoveLinks: false,
                dictDefaultMessage: 'Drag & drop a file here, or click to upload',
                headers: {
                    'RequestVerificationToken': token
                },
                init: function () {
                    this.on('success', function () {
                        loadDocuments();
                    });
                    this.on('removedfile', function () {
                        // no-op: removal handled via the document list below
                    });
                }
            });
        }

        loadDocuments();
    });
})();
