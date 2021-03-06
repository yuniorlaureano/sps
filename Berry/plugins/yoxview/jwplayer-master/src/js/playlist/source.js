define([
    'utils/helpers',
    'utils/strings',
    'utils/underscore'
], function(utils, strings, _) {

    var Defaults = {
        //file: undefined,
        //label: undefined,
        //type: undefined,
        //androidhls : undefined,
        'default': false
    };


    var Source = function (config) {
        // file is the only hard requirement
        if (!config || !config.file) {
            return;
        }

        var _source = _.extend({}, Defaults, config);

        // normalize for odd strings
        _source.file = strings.trim('' + _source.file);

        // regex to check if mimetype is given
        var mimetypeRegEx = /^[^\/]+\/(?:x-)?([^\/]+)$/;

        // check if the source is youtube or rtmp
        if (utils.isYouTube(_source.file)) {
            _source.type = 'youtube';
        } else if (utils.isRtmp(_source.file)) {
            _source.type = 'rtmp';
        } else if (mimetypeRegEx.test(_source.type)) {
            // if type is given as a mimetype
            _source.type = _source.type.replace(mimetypeRegEx, '$1');
        } else if (!_source.type) {
            _source.type = strings.extension(_source.file);
        }

        if (!_source.type) {
            return;
        }

        // normalize types
        if (_source.type === 'm3u8') {
            _source.type = 'hls';
        }
        if (_source.type === 'smil') {
            _source.type = 'rtmp';
        }
        // Although m4a is a container format, it is most often used for aac files
        // http://en.wikipedia.org/w/index.php?title=MPEG-4_Part_14
        if (_source.type === 'm4a') {
            _source.type = 'aac';
        }

        // remove empty strings
        _.each(_source, function(val, key) {
            if (val === '') {
                delete _source[key];
            }
        });

        return _source;
    };

    return Source;
});
