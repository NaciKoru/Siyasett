!(function () {
    var t = sessionStorage.getItem("__HYPER_CONFIG__"),
        e = document.getElementsByTagName("html")[0],
        i = {
            theme: "light",
            nav: "vertical",
            layout: { mode: "fluid", position: "fixed" },
            topbar: { color: "dark" },
            sidenav: { color: "dark", size: "condensed", user: !0 }
        },
        o = ((this.html = document.getElementsByTagName("html")[0]), (config = Object.assign( JSON.parse( JSON.stringify( i)), {})), this.html.getAttribute("data-theme")),
        o = ((config.theme = null !== o ? o : i.theme), this.html.getAttribute("data-layout")),
        o = ((config.nav = null !== o ? ("topnav" === o ? "horizontal" : "vertical") : i.nav), this.html.getAttribute("data-layout-mode")),
        o = ((config.layout.mode = null !== o ? o : i.layout.mode), this.html.getAttribute("data-layout-position")),
        o = ((config.layout.position = null !== o ? o : i.layout.position), this.html.getAttribute("data-topbar-color")),
        o = ((config.topbar.color = null != o ? o : i.topbar.color), this.html.getAttribute("data-sidenav-color")),
        o = ((config.sidenav.color = null !== o ? o : i.sidenav.color), this.html.getAttribute("data-sidenav-size")),
        o = ((config.sidenav.size = null !== o ? o : i.sidenav.size), this.html.getAttribute("data-sidenav-user"));
    (config.sidenav.user = null !== o || i.sidenav.user),
        (window.defaultConfig = JSON.parse(JSON.stringify(config))),
        //null !== t && (config = JSON.parse(t)),
        (window.config = config),
        "topnav" === e.getAttribute("data-layout") ? (config.nav = "horizontal") : (config.nav = "vertical"),
        config && true;
        (e.setAttribute("data-theme", config.theme),
            e.setAttribute("data-layout-mode", config.layout.mode),
            e.setAttribute("data-topbar-color", config.topbar.color),
            "vertical" == config.nav &&
            (e.setAttribute("data-sidenav-size", config.sidenav.size),
                e.setAttribute("data-sidenav-color", config.sidenav.color),
                e.setAttribute("data-layout-position", config.layout.position),
                config.sidenav.user && "true" === config.sidenav.user.toString() ? e.setAttribute("data-sidenav-user", !0) : e.removeAttribute("data-sidenav-user"))

        );
})();
