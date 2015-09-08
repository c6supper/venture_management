var SEARCH_URL = "/search/",
    lockHistoryChange = false;

var makeTab = function (id, url, title) {
    var win, 
        tab, 
        hostName, 
        menuName, 
        node, 
        tabTip;
    
    if (id === "-") {
        id = Ext.id(undefined, "extnet");
        lookup[url] = id;
    }
    
    tabTip = url.replace(/^\//g, "");
    tabTip = tabTip.replace(/\/$/g, "");
    tabTip = tabTip.replace(/\//g, " > ");
    tabTip = tabTip.replace(/_/g, " ");
    
    win = new Ext.window.Window({
        id      : "w" + id,
        layout  : "fit",        
        title   : "Source Code",
        iconCls : "#PageWhiteCode",
        width   : 925,
        height  : 650,
        border  : false,
        maximizable : true,
        constrain   : true,
        closeAction : "hide",        
        listeners   : {
            show : {
                fn : function () {
                    var me = this,
                        height = Ext.getBody().getViewSize().height;
                    
                    if (this.getSize().height > height) {
                        this.setHeight(height - 20)
                    }

                    App.direct.GetSourceTabs(id, url, this.nsId, {
                        eventMask: {
                            showMask : true,
                            customTarget : this.body
                        },
                        failure : function (msg, response) {
                            Ext.Msg.alert("Failure", "The error during menu loading:\n" + response.responseText);
                        }
                    });
                },
                
                single : true
            }
        },
        buttons :[
            {
                id   : "b" + id,
                text : "Download",
                iconCls   : "#Compress",
                listeners : {
                    click : {
                        fn : function (el, e) {
                            App.direct.DownloadMenu(url, {
                                isUpload: true,
                                formId : "downloadForm"
                            });
                        }
                    }
                }
            }
        ]        
    });
    
    hostName = window.location.protocol + "//" + window.location.host;
    menuName = url;
    
    tab = App.MenuTabs.add(new Ext.panel.Panel({
        id   : id,        
        tbar: [
        //    {
        //    text      : "Source Code",
        //    iconCls   : "#PageWhiteCode",
        //    listeners : {
        //        "click" : function () {
        //            Ext.getCmp("w" + id).show(null);
        //        }
        //    }
        //},        
        "->", 
	    //{
        //    text    : "Direct Link",
        //    iconCls : "#Link",
        //    handler : function () {
        //        new Ext.window.Window({
        //            modal     : true,
        //            iconCls   : "#Link",
        //            layout    : "absolute",
        //            defaultButton : "dl" + id,
        //            width     : 400,
        //            height    : 110,
        //            title     : "Direct Link",
        //            closable  : false,
        //            resizable : false,
        //            items : [{
        //                id    : "dl" + id,
        //                xtype : "textfield",
        //                cls   : "dlText",
        //                width : 364,
        //                x     : 10,
        //                y     : 10,
        //                selectOnFocus : true,
        //                readOnly : true,
        //                value    : hostName + "/#" + menuName
        //            }],
        //            buttons: [{
        //                xtype   : "button",
        //                text    : " Open",
        //                iconCls : "#ApplicationDouble",
        //                tooltip : "Open Menu in the separate window",
        //                handler : function () {
        //                    window.open(hostName + "/#" + menuName);
        //                }
        //            },
        //            {
        //                xtype   : "button",
        //                text    : " Open (Direct)",
        //                iconCls : "#ApplicationGo",
        //                tooltip : "Open Menu in the separate window using a direct link",
        //                handler : function () {
        //                    window.open(hostName + url, "_blank");
        //                }
        //            },
        //            {
        //                xtype   : "button",
        //                text    : "Close",
        //                handler : function () {
        //                    this.findParentByType("window").hide(null);
        //                }
        //            }]
        //        }).show(null);
        //    }
        //},
        "-", 
        {
            text    : "刷新",
            handler : function () {
                Ext.getCmp(id).reload(true)
            },
            iconCls : "#ArrowRefresh"
        }],
        title    : title,
        tabTip   : tabTip,
        hideMode : "offsets",        

        loader : {            
            renderer : "frame",
            url      : hostName + url,
            loadMask : true
        },
        listeners : {
            deactivate : {
                fn : function (el) {
                    if (this.sWin && this.sWin.isVisible()) {
                         this.sWin.hide();
                    }
                }
            },
            
            destroy : function () {
                if (this.sWin) {
                    this.sWin.close();
                    this.sWin.destroy();
                }
            }
        },
        closable : true
    }));
    
    tab.sWin = win;
    setTimeout(function(){
        App.MenuTabs.setActiveTab(tab);
    }, 250);
    
    var node = App.menuTree.getStore().getNodeById(id),
        expandAndSelect = function (node) {
            App.menuTree.animate = false;
            node.bubble(function(node) {
                node.expand(false);
            }); 
            App.menuTree.getSelectionModel().select(node);       
            App.menuTree.animate = true;    
        };
         
    if (node) {
        expandAndSelect(node);     
    } else {
        App.menuTree.on("load", function (node) {
            node = App.menuTree.getStore().getNodeById(id);
            if (node) {
                expandAndSelect(node);
            }
        }, this, { delay: 10, single : true });
    }
};

var lookup = {};

var onTreeAfterRender = function (tree) {
    var sm = tree.getSelectionModel();

    Ext.create('Ext.util.KeyNav', tree.view.el, {
        enter : function (e) {
            if (sm.hasSelection()) {
                onTreeItemClick(sm.getSelection()[0], e);
            }
        }
    });
};

var onTreeItemClick = function (record, e) {
    if (record.isLeaf()) { 
        e.stopEvent(); 
        loadMenu(record.get('href'), record.getId(), record.get('text')); 
    } else {
        record[record.isExpanded() ? 'collapse' : 'expand']();
    }
};

var treeRenderer = function (value, metadata, record) {
    value = record.data.text; // It looks an ExtJS bug - value is undefined.

    if (record.data.isNew) {
        value += "<span>&nbsp;</span>";
    }
    
    return value;
};

var loadMenu = function (href, id, title) {
    var tab = App.MenuTabs.getComponent(id),
        lObj = lookup[href];
        
    if (id == "-") {
        App.direct.GetHashCode(href,{
            success : function (result) {
                loadMenu(href, "e" + result, title);
            }
        });
               
        return;
    }
    
    lookup[href] = id;

    if (tab) {
        App.MenuTabs.setActiveTab(tab);
    } else {
        if (Ext.isEmpty(title)) {
            var m = /(\w+)\/$/g.exec(href);
            title = m == null ? "[No name]" : m[1];
        }
        
        title = title.replace(/<span>&nbsp;<\/span>/g, "");
        title = title.replace(/_/g, " ");
        makeTab(id, href, title);     
    }
};

var viewClick = function (dv, e) {
    var group = e.getTarget("h2", 3, true);

    if (group) {
        group.up("div").toggleClass("collapsed");
    }
};

var beforeSourceShow = function (el) {
    var height = Ext.getBody().getViewSize().height;
    
    if (el.getSize().height > height) {
        el.setHeight(height - 20);
    }
};

var change = function (token) {
    if (!lockHistoryChange) {
        if (token) {
            if (token.indexOf(SEARCH_URL) === 0) {
                filterByUrl(token);
            } else {
                loadMenu(token, lookup[token] || "-" );
            }
        } else {
            App.MenuTabs.setActiveTab(0);
        }
    }
    lockHistoryChange = false;
};

var getToken = function (url) {
    var host = window.location.protocol + "//" + window.location.host;

    return url.substr(host.length);
};

var addToken = function (el, tab) {
    if (tab.loader && tab.loader.url) {
        var token = getToken(tab.loader.url);        
        if (!Ext.isEmpty(token)) {
            Ext.History.add(token);
        }
    } else {                
        Ext.History.add("");                
    }
};

var keyUp = function (field, e) {
    if (e.getKey() === 40) {
        return;
    }

    if (e.getKey() === Ext.EventObject.ESC) {
        clearFilter(field);
    } else {
        changeFilterHash(field.getRawValue());            
        filter(field, e);
    }
};

var filter = function (field, e) {    
    var tree = App.menuTree,
        text = field.getRawValue();
    
    if (Ext.isEmpty(text, false)) {
        clearFilter(field);
    }
    
    if (text.length < 3) {
        return;
    }    
    
    if (Ext.isEmpty(text, false)) {
        return;
    }
    
    field.getTrigger(0).show();
    
    if (e && e.getKey() === Ext.EventObject.ESC) {
        clearFilter(field);
    } else {
        var re = new RegExp(".*" + text + ".*", "i");
        
        tree.clearFilter(true);
        
        tree.filterBy(function (node) {
            var match = re.test(node.data.text.replace(/<span>&nbsp;<\/span>/g, "")),
                pn = node.parentNode;
                
            if (match && node.isLeaf()) {
               pn.hasMatchNode = true;
            }
            
            if (pn != null && pn.fixed) {
                if (node.isLeaf() === false) {
                    node.fixed = true;
                }
                return true;
            }            
                
            if (node.isLeaf() === false) {
                node.fixed = match;
                return match;
            }            
            
            return (pn != null && pn.fixed) || match;
        }, { expandNodes : false });

        tree.getView().animate = false;
        tree.getRootNode().cascadeBy(function (node) {
            if (node.isRoot()) {
               return;
            }            
            
            if ((node.getDepth() === 1) || 
               (node.getDepth() === 2 && node.hasMatchNode)) {
               node.expand(false);
            }
            
        delete node.fixed;
        delete node.hasMatchNode;
      }, tree);
      tree.getView().animate = true;
    }
};

var filterByUrl = function (url) {
    var field = App.SearchField,
        tree = App.menuTree;

    if (!lockHistoryChange) {
        var tree = App.menuTree,
            store = tree.getStore(),
            fn = function () {
                field.setValue(url.substr(SEARCH_URL.length));
                filter(field);
            };

        if (store.loading) {
            store.on("load", fn, null, { single : true });
        } else {
            fn();
        }
    }
};

var clearFilter = function (field, trigger, index, e) {
    var tree = App.menuTree;
    
    field.setValue("");
    changeFilterHash("");
    field.getTrigger(0).hide();
    tree.clearFilter(true);     
    field.focus(false, 100);        
};

var changeFilterHash = Ext.Function.createBuffered(
    function (text) {
        lockHistoryChange = true;
        if (text.length > 2) {
            window.location.hash = SEARCH_URL + text;
        } else {
            var tab = App.MenuTabs.getActiveTab(),
                token = "";

            if (tab.loader && tab.loader.url) {
                token = getToken(tab.loader.url);
            }
        
            Ext.History.add(token);
        }
    },
    500);

var filterSpecialKey = function (field, e) {
    if (e.getKey() === e.DOWN) {
        var n = App.menuTree.getRootNode().findChildBy(function (node) {
            return node.isLeaf() && !node.data.hidden;
        }, App.menuTree, true);
        
        if (n) {
            App.menuTree.expandPath(n.getPath(), null, null, function(){
                App.menuTree.getSelectionModel().select(n);
                App.menuTree.getView().focus();
            });
        }
    }
};

var filterNewMenus = function (checkItem, checked) {
    var tree = App.menuTree,
        regex;
        
    if (checked) {
        tree.clearFilter(true);
        regex = new RegExp("<span>&nbsp;</span>");
        tree.filterBy(function (node) {
            return regex.test(node.data.text);
        });    
    } else {
        tree.clearFilter(true);
    }
};

var swapThemeClass = function (frame, oldTheme, newTheme) {
    var html = Ext.fly(frame.document.body.parentNode);
                                                                        
    html.removeCls('x-theme-' + oldTheme);
    html.addCls('x-theme-' + newTheme);
};

var themeChange = function (menu, menuItem) {
    App.direct.GetThemeUrl(menuItem.text, {
		success : function (result) {
            var v = menu.up('viewport'),
                oldTheme = Ext.net.ResourceMgr.theme,
                html,
                frame;
																			
            Ext.net.ResourceMgr.setTheme(result, menuItem.text.toLowerCase());
            Ext.defer(v.doLayout, 500, v);
			App.MenuTabs.items.each(function (el) {
				if (!Ext.isEmpty(el.iframe)) {
                    frame = el.getBody();
					if (frame.Ext) {
						frame.Ext.net.ResourceMgr.setTheme(result, menuItem.text.toLowerCase());
					} else {
                        swapThemeClass(frame, oldTheme, Ext.net.ResourceMgr.theme);                                                                                                                                                                
                    }
				}
			});
		}
	});
};

if (window.location.href.indexOf("#") > 0) {
    var directLink = window.location.href.substr(window.location.href.indexOf("#") + 1);
    
    Ext.onReady(function () {
        Ext.Function.defer(function(){
            if (directLink.indexOf(SEARCH_URL) === 0) {
                filterByUrl(directLink);                
            } else {
                if (!Ext.isEmpty(directLink, false)) {
                    loadMenu(directLink, "-");
                }
            }
        }, 100, window);        
    }, window);
}