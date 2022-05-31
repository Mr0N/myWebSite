var Wpfcll = {
    s: [],
    osl: 0,
    scroll: false,
    i: function() {
        Wpfcll.ss();
        window.addEventListener('load', function() {
            window.addEventListener("DOMSubtreeModified", function(e) {
                Wpfcll.osl = Wpfcll.s.length;
                Wpfcll.ss();
                if (Wpfcll.s.length > Wpfcll.osl) {
                    Wpfcll.ls(false);
                }
            }, false);
            Wpfcll.ls(true);
        });
        window.addEventListener('scroll', function() {
            Wpfcll.scroll = true;
            Wpfcll.ls(false);
        });
        window.addEventListener('resize', function() {
            Wpfcll.scroll = true;
            Wpfcll.ls(false);
        });
        window.addEventListener('click', function() {
            Wpfcll.scroll = true;
            Wpfcll.ls(false);
        });
    },
    c: function(e, pageload) {
        var w = document.documentElement.clientHeight || body.clientHeight;
        var n = 0;
        if (pageload) {
            n = 0;
        } else {
            n = (w > 800) ? 800 : 200;
            n = Wpfcll.scroll ? 800 : n;
        }
        var er = e.getBoundingClientRect();
        var t = 0;
        var p = e.parentNode ? e.parentNode : false;
        if (typeof p.getBoundingClientRect == "undefined") {
            var pr = false;
        } else {
            var pr = p.getBoundingClientRect();
        }
        if (er.x == 0 && er.y == 0) {
            for (var i = 0; i < 10; i++) {
                if (p) {
                    if (pr.x == 0 && pr.y == 0) {
                        if (p.parentNode) {
                            p = p.parentNode;
                        }
                        if (typeof p.getBoundingClientRect == "undefined") {
                            pr = false;
                        } else {
                            pr = p.getBoundingClientRect();
                        }
                    } else {
                        t = pr.top;
                        break;
                    }
                }
            };
        } else {
            t = er.top;
        }
        if (w - t + n > 0) {
            return true;
        }
        return false;
    },
    r: function(e, pageload) {
        var s = this;
        var oc, ot;
        try {
            oc = e.getAttribute("data-wpfc-original-src");
            ot = e.getAttribute("data-wpfc-original-srcset");
            if (s.c(e, pageload)) {
                if (oc || ot) {
                    if (e.tagName == "DIV" || e.tagName == "A") {
                        e.style.backgroundImage = "url(" + oc + ")";
                        e.removeAttribute("data-wpfc-original-src");
                        e.removeAttribute("data-wpfc-original-srcset");
                        e.removeAttribute("onload");
                    } else {
                        if (oc) {
                            e.setAttribute('src', oc);
                        }
                        if (ot) {
                            e.setAttribute('srcset', ot);
                        }
                        if (e.getAttribute("alt") && e.getAttribute("alt") == "blank") {
                            e.removeAttribute("alt");
                        }
                        e.removeAttribute("data-wpfc-original-src");
                        e.removeAttribute("data-wpfc-original-srcset");
                        e.removeAttribute("onload");
                        if (e.tagName == "IFRAME") {
                            e.onload = function() {
                                if (typeof window.jQuery != "undefined") {
                                    if (jQuery.fn.fitVids) {
                                        jQuery(e).parent().fitVids({
                                            customSelector: "iframe[src]"
                                        });
                                    }
                                }
                                var s = e.getAttribute("src").match(/templates\/youtube\.html\#(.+)/);
                                var y = "https://www.youtube.com/embed/";
                                if (s) {
                                    try {
                                        var i = e.contentDocument || e.contentWindow;
                                        if (i.location.href == "about:blank") {
                                            e.setAttribute('src', y + s[1]);
                                        }
                                    } catch (err) {
                                        e.setAttribute('src', y + s[1]);
                                    }
                                }
                            }
                        }
                    }
                } else {
                    if (e.tagName == "NOSCRIPT") {
                        if (jQuery(e).attr("data-type") == "wpfc") {
                            e.removeAttribute("data-type");
                            jQuery(e).after(jQuery(e).text());
                        }
                    }
                }
            }
        } catch (error) {
            console.log(error);
            console.log("==>", e);
        }
    },
    ss: function() {
        var i = Array.prototype.slice.call(document.getElementsByTagName("img"));
        var f = Array.prototype.slice.call(document.getElementsByTagName("iframe"));
        var d = Array.prototype.slice.call(document.getElementsByTagName("div"));
        var a = Array.prototype.slice.call(document.getElementsByTagName("a"));
        var n = Array.prototype.slice.call(document.getElementsByTagName("noscript"));
        this.s = i.concat(f).concat(d).concat(a).concat(n);
    },
    ls: function(pageload) {
        var s = this;
        [].forEach.call(s.s, function(e, index) {
            s.r(e, pageload);
        });
    }
};
document.addEventListener('DOMContentLoaded', function() {
    wpfci();
});

function wpfci() {
    Wpfcll.i();
}