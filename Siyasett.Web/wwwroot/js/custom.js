var $ = jQuery.noConflict();

function image_preload(selector, parameters) {
	var params = {
		delay: 250,
		transition: 400,
		easing: 'linear'
	};
	$.extend(params, parameters);
		
	$(selector).each(function() {
		var image = $(this);
		image.css({visibility:'hidden', opacity: 0, display:'block'});
		image.wrap('<span class="preloader" />');
		image.one("load", function(evt) {
			$(this).delay(params.delay).css({visibility:'visible'}).animate({opacity: 1}, params.transition, params.easing, function() {
				$(this).unwrap('<span class="preloader" />');
			});
		}).each(function() {
			if(this.complete) $(this).trigger("load");
		});
	});
}


function tab_widget(tabid) {
    
    var $sidebarWidgets = $('.sidebar-widgets-wrap');
    var $footerWidgets = $('.footer-widgets-wrap');
    
    $( tabid + " .tab_content").hide();
    $( tabid + " ul.tabs li:first").addClass("active").show();
    $( tabid + " .tab_content:first").show();
    
    if( window.location.hash != '' ) {
        
        var getTabHash = window.location.hash;
        
        if( !getTabHash.match( /#filter=/gi ) ) {
        
            if( $( getTabHash ).hasClass('tab_content') ) {
            
                $( tabid + " ul.tabs li").removeClass("active");
                $( tabid + ' ul.tabs li a[data-href="'+ getTabHash +'"]').parent('li').addClass("active");
                $( tabid + " .tab_content").hide();
                $( getTabHash + '.tab_content').show();
            
            }
        
        }
        
    }
    
    $( tabid + " ul.tabs li").click(function() {
		
        $( tabid + " ul.tabs li").removeClass("active");
        $(this).addClass("active");		
        $( tabid + " .tab_content").hide();
        var activeTab = $(this).find("a").attr("data-href");
        var $selectTab = $(this);
        $(activeTab).fadeIn(600,function(){
            if( $selectTab.parent().parent().hasClass("side-tabs") ) {
                if( $(window).width() < 768 ) { if( $().scrollTo ) { jQuery.scrollTo( activeTab , 400, {offset:-20} ); } }
            }
            $( '.flexslider .slide' ).resize();
        });
        return false;
        
	});
    
}


jQuery(document).ready(function($) {
                
        
        // Dropdown Menu
        
        if ( $().superfish ) {
        
            $("#primary-menu ul, .sticky-menu-wrap ul, #top-menu ul").superfish({ 
                delay: 250,
                speed: 300,
                animation: {opacity:'show', height:'show'},
                autoArrows: false
            });
        
        }
        
        
        // ToolTips
        
        if ( $().tipsy ) { nTip=function(){ $('.ntip').tipsy({gravity: 's', fade:true}); }; nTip(); }
		if ( $().tipsy ) { sTip=function(){ $('.stip').tipsy({gravity: 'n', fade:true}); }; sTip(); }
		if ( $().tipsy ) { eTip=function(){ $('.etip').tipsy({gravity: 'w', fade:true}); }; eTip(); }
		if ( $().tipsy ) { wTip=function(){ $('.wtip').tipsy({gravity: 'e', fade:true}); }; wTip(); }
        
        
        $("#primary-menu ul li:has(ul)").addClass('sub-menu');
        
        $(".sticky-menu-wrap ul li:has(ul)").addClass('sub-menu');
        
        
        var headerHeight = $('#header').outerHeight() + 170;
        
        stickyMenuFunction=function(){
        
        var scrollTimer = null;
        $(window).scroll(function () {
            if (scrollTimer) {
                clearTimeout(scrollTimer);
            }
            scrollTimer = setTimeout(handleScroll, 200);
        });
        
        function handleScroll() {
            scrollTimer = null;
            
            var stickyWindowWidth = $(window).width();
            
            if( stickyWindowWidth > 979 ) {
            
                if ($(window).scrollTop() > headerHeight) {
                    $('#sticky-menu').show();
                    $('#sticky-menu').filter(':not(:animated)').animate({top:'0px'}, 250);
                } else {
                    $('#sticky-menu').filter(':not(:animated)').animate({top:'-60px'}, 250, function(){
                        $(this).fadeOut();
                    });
                }
            
            } else {
                $('#sticky-menu').hide();
            }
        }
        
        };
		stickyMenuFunction();
        
        
        $('.sticky-search-trigger a').click(function() {
			$('.sticky-search-area').fadeIn('fast', function(){
                $(this).find('input').focus();
			});
            return false;
		});
        
        $('.sticky-search-area-close a').click(function() {
			$('.sticky-search-area').fadeOut('fast');
            return false;
		});


        // Scroll to Top
        
		$(window).scroll(function() {
			if($(this).scrollTop() > 450) {
                $('#gotoTop').fadeIn();
			} else {
				$('#gotoTop').fadeOut();
			}
		});
        
        
		$('#gotoTop').click(function() {
			$('body,html').animate({scrollTop:0},400);
            return false;
		});
        
        
        $(".rs-menu").click(function () {
            $("#primary-menu > ul, #primary-menu > div > ul").slideToggle(500);
            return false;
        });
        
        if ($(window).width() < 980) {
            $("#primary-menu > ul, #primary-menu > div > ul").hide();
        }
        
        $(window).resize(function () {
             if ($(window).width() < 980) {
                 $("#primary-menu > ul, #primary-menu > div > ul").hide();
             }
             if ($(window).width() > 980) {
                 $('#primary-menu > ul, #primary-menu > div > ul').show();
             }
        });
        
        
        
        
        // Top Socials
        
        topSocialExpander=function(){
            
            var windowWidth = $(window).width();
        
            if( windowWidth > 767 ) {
            
                $("#top-social li").show();
                
                $("#top-social li a").css({width: 40});
                
                $("#top-social li a").each(function() {
                    $(this).removeClass('stip');
                    $(this).removeAttr('title');
                    $(this).removeAttr('original-title');
                });
                
                $("#top-social li a").hover(function() {
                    var tsTextWidth = $(this).children('.ts-text').outerWidth() + 52;
        			$(this).stop().animate({width: tsTextWidth}, 250, 'jswing');
        		}, function() {
        			$(this).stop().animate({width: 40}, 250, 'jswing');
        		});
            
            } else {
                
                $("#top-social li").show();
                
                $("#top-social li a").css({width: 40});
                
                $("#top-social li a").each(function() {
                    $(this).addClass('stip');
                    var topIconTitle = $(this).children('.ts-text').text();
                    $(this).attr('title', topIconTitle);
                });
                
                sTip();
                
                $("#top-social li a").hover(function() {
                    $(this).stop().animate({width: 40}, 1, 'jswing');
        		}, function() {
        			$(this).stop().animate({width: 40}, 1, 'jswing');
        		});
                
                if( windowWidth < 479 ) {
                    
                    $("#top-social li").hide();
                    $("#top-social li").slice(0, 8).show();
                    
                }
                
            }
        
        };
		topSocialExpander();
        
        $(window).resize(function() {
            topSocialExpander();
            stickyMenuFunction();
        });




        
        // Siblings Fader
        
        siblingsFader=function(){
		$(".siblings_fade,.flickr_badge_image").hover(function() {
			$(this).siblings().stop().fadeTo(400,0.5);
		}, function() {
			$(this).siblings().stop().fadeTo(400,1);
		});
		};
		siblingsFader();
        

        
        
        // Image Fade
        
		imgFade=function(){
		$('.image_fade,#top-menu li.top-menu-em a').hover(function(){
			$(this).filter(':not(:animated)').animate({opacity: 0.6}, 400);
		}, function () {
			$(this).animate({opacity: 1}, 400);
		});
		};
		imgFade();
        
        
        $(window).scroll(function () {
        
            $('.progress:in-viewport').each(function(){
    			var skillsBar = $(this),
    			skillValue = skillsBar.find('.progress-bar').attr('data-width');
    			if (!skillsBar.hasClass('animated')) {
                    skillsBar.parent().find('span').hide();
    				skillsBar.addClass('animated');
    				skillsBar.find('.progress-bar').animate({
    					width: skillValue + "%"
    				}, 500, function() {
    					skillsBar.parent().find('span').fadeIn(400);
    				});
    			}
    		});
        
        });
        
        
        // Toggles
        
        $(".togglec").hide();
    	
    	$(".togglet").click(function(){
    	
    	   $(this).toggleClass("toggleta").next(".togglec").slideToggle("normal");
    	   return true;
        
    	});
        
        


                
        
        // Portfolio Hoverlay
        
        imgHoverlay=function(){
		$('.portfolio-item,#portfolio-related-items li').hover(function(){
			$(this).find('.portfolio-overlay').filter(':not(:animated)').animate({opacity: 'show'}, 400);
		}, function () {
			$(this).find('.portfolio-overlay').animate({opacity: 'hide'}, 400);
		});
		};
		imgHoverlay();
        
        
        // FitVids
        
        if ( $().fitVids ) { $("#content,#footer,#slider:not(.layerslider-wrap),.landing-offer-media").fitVids( { customSelector: "iframe[src^='http://www.dailymotion.com/embed']"} ); }
        
        
        // prettyPhoto
        
        if ( $().prettyPhoto ) {
            
            initprettyPhoto=function(){
                
                $("a[rel^='prettyPhoto']").prettyPhoto({ theme: 'light_square', social_tools: false });
            
            };
            initprettyPhoto();
        
        }
        
        
        // Mobile Menu
        
        if( $().mobileMenu ) { $('#primary-menu ul#main-menu').mobileMenu({ subMenuDash: '&nbsp;&ndash;&nbsp;' }); }
        
        
        // UniForm
        
        // if( $().uniform ) { $("#primary-menu select").uniform({selectClass: 'rs-menu'}); }
        
        
        // Anchor Link Scroll
        
        $("a[data-scrollto]").click(function(){
    	
            var divScrollToAnchor = $(this).attr('data-scrollto');
            
            if( $().scrollTo ) { jQuery.scrollTo( $( divScrollToAnchor ) , 400, {offset:-20} ); }
            
            return false;
        
    	});


        $('[data-animate]').each(function(){

            var $toAnimateElement = $(this);

            var toAnimateDelay = $(this).attr('data-delay');

            var toAnimateDelayTime = 0;

            if( toAnimateDelay ) { toAnimateDelayTime = Number( toAnimateDelay ) + 500; } else { toAnimateDelayTime = 500; }

            if( !$toAnimateElement.hasClass('animated') ) {

                $toAnimateElement.addClass('not-animated');

                var elementAnimation = $toAnimateElement.attr('data-animate');
                
                $toAnimateElement.appear(function () {

                    setTimeout(function() {
                        $toAnimateElement.removeClass('not-animated').addClass( elementAnimation + ' animated');
                    }, toAnimateDelayTime);

                },{accX: 0, accY: -80},'easeInCubic');

            }
        
        });


        // Magnific Lightbox

        loadMagnific=function(){

        $('[data-lightbox="image"]').magnificPopup({
            type: 'image',
            closeOnContentClick: true,
            closeBtnInside: false,
            fixedContentPos: true,
            mainClass: 'mfp-no-margins mfp-with-zoom', // class to remove default margin from left and right side
            image: {
                verticalFit: true
            },
            zoom: {
                enabled: true, // By default it's false, so don't forget to enable it

                duration: 300, // duration of the effect, in milliseconds
                easing: 'ease-in-out', // CSS transition easing function
                opener: function(openerElement) {
                  return openerElement.is('img') ? openerElement : openerElement.parent().parent().parent().find('img');
                }
              }
        });


        $('[data-lightbox="gallery"]').each(function() {

            if( $(this).find('a[data-lightbox="gallery-item"]').parent('.clone').hasClass('clone') ) {
                $(this).find('a[data-lightbox="gallery-item"]').parent('.clone').find('a[data-lightbox="gallery-item"]').attr('data-lightbox','');
            }

            $(this).magnificPopup({
                delegate: 'a[data-lightbox="gallery-item"]',
                type: 'image',
                closeOnContentClick: true,
                closeBtnInside: false,
                fixedContentPos: true,
                mainClass: 'mfp-no-margins mfp-with-zoom', // class to remove default margin from left and right side
                image: {
                    verticalFit: true
                },
                gallery: {
                    enabled: true,
                    navigateByImgClick: true,
                    preload: [0,1] // Will preload 0 - before current, and 1 after the current image
                },
                zoom: {
                    enabled: true, // By default it's false, so don't forget to enable it
                    duration: 300, // duration of the effect, in milliseconds
                    easing: 'ease-in-out', // CSS transition easing function
                    opener: function(openerElement) {
                      return openerElement.is('img') ? openerElement : openerElement.parent().parent().parent().find('img');
                    }
                  }
            });

        });


        $('[data-lightbox="iframe"]').magnificPopup({
            disableOn: 700,
            type: 'iframe',
            mainClass: 'mfp-fade',
            removalDelay: 160,
            preloader: false,
            fixedContentPos: false
        });

        };
        loadMagnific();
        
        
        // Testimonials
        
        if( $().carouFredSel ) {
            
            $('.testimonials').each(function() {
                
                var testimonialLeftKey = $(this).parent('.testimonial-scroller').attr('data-prev');
                var testimonialRightKey = $(this).parent('.testimonial-scroller').attr('data-next');
                var testimonialSpeed = $(this).parent('.testimonial-scroller').attr('data-speed');
                var testimonialPause = $(this).parent('.testimonial-scroller').attr('data-pause');
                var testimonialAuto = $(this).parent('.testimonial-scroller').attr('data-auto');
                
                if( !testimonialSpeed ) { testimonialSpeed = 300; }
                if( !testimonialPause ) { testimonialPause = 8000; }
                if( testimonialAuto == 'true' ) { testimonialAuto = Number(testimonialPause); } else { testimonialAuto = false; }
                
                $(this).carouFredSel({
                	circular : true,
                    responsive : true,
                    auto : testimonialAuto,
                    items : 1,
                	scroll : {
                		items : "page",
                        fx : "fade",
                        duration : Number(testimonialSpeed),
                        wipe : true
                	},
                	prev : {
                		button : testimonialLeftKey,
                		key : "left"
                	},
                	next : {
                		button : testimonialRightKey,
                		key : "right"
                	}
                });
            
            });
        
        }
        
        
        // Flickr Feed
        
        if( $().jflickrfeed ) {
            
            $('.flickrfeed').each(function() {
                
                var flickrFeedID = $(this).attr('data-id');
                var flickrFeedCount = $(this).attr('data-count');
                var flickrFeedType = $(this).attr('data-type');
                var flickrFeedTypeGet = 'photos_public.gne';
                
                if( flickrFeedType == 'group' ) { flickrFeedTypeGet = 'groups_pool.gne'; }
                
                if( !flickrFeedCount ) { flickrFeedCount = 9; }
            
                $(this).jflickrfeed({
                    feedapi: flickrFeedTypeGet,
            		limit: Number(flickrFeedCount),
            		qstrings: {
            			id: flickrFeedID
            		},
            		itemTemplate: '<div class="flickr_badge_image">'+
            						'<a rel="prettyPhoto[galflickr]" href="{{image}}" title="{{title}}">' +
            							'<img src="{{image_s}}" alt="{{title}}" />' +
            						'</a>' +
            					  '</div>'
            	}, function(data) {
            		if ( $().prettyPhoto ) { initprettyPhoto(); }
            	});
            
            });
            
        }
        
        
        // Instagram Photos
        
        if( $().spectragram ) {
        
            $.fn.spectragram.accessData = {
                accessToken: '36286274.b9e559e.4824cbc1d0c94c23827dc4a2267a9f6b', // your Instagram Access Token
                clientID: 'b9e559ec7c284375bf41e9a9fb72ae01' // Your Client ID
            };
            
            $('.instagram').each(function() {
                
                var instaGramUsername = $(this).attr('data-user');
                var instaGramTag = $(this).attr('data-tag');
                var instaGramCount = $(this).attr('data-count');
                var instaGramType = $(this).attr('data-type');
                
                if( !instaGramCount ) { instaGramCount = 9; }
                
                if( instaGramType == 'tag' ) {
                
                    $(this).spectragram('getRecentTagged',{
                        query: instaGramTag,
                        max: Number( instaGramCount ),
                        size: 'small',
                        wrapEachWith: '<div class="flickr_badge_image"></div>'
                    });
                
                } else if( instaGramType == 'user' ) {
                    
                    $(this).spectragram('getUserFeed',{
                        query: instaGramUsername,
                        max: Number( instaGramCount ),
                        size: 'small',
                        wrapEachWith: '<div class="flickr_badge_image"></div>'
                    });
                    
                } else {
                    
                    $(this).spectragram('getPopular',{
                        max: Number( instaGramCount ),
                        size: 'small',
                        wrapEachWith: '<div class="flickr_badge_image"></div>'
                    });
                    
                }
            
            });
        
        }
        
        
        // Dribbble Shots
        
        if( $().jribbble ) {
            
            
            $('.dribbble').each(function() {
                
                var dribbbleWrap = $(this);
                var dribbbleUsername = $(this).attr('data-user');
                var dribbbleCount = $(this).attr('data-count');
                var dribbbleList = $(this).attr('data-list');
                var dribbbleType = $(this).attr('data-type');
                
                if( !dribbbleCount ) { dribbbleCount = 9; }
            
                if( dribbbleType == 'follows' ) {
                
                    $.jribbble.getShotsThatPlayerFollows( dribbbleUsername , function (followedShots) {
                        var html = [];
                    
                        $.each(followedShots.shots, function (i, shot) {
                            html.push('<div class="flickr_badge_image"><a href="' + shot.url + '" target="_blank">');
                            html.push('<img src="' + shot.image_teaser_url + '" ');
                            html.push('alt="' + shot.title + '"></a></div>');
                        });
                    
                        $(dribbbleWrap).html(html.join(''));
                    }, {page: 1, per_page: Number(dribbbleCount)});
                
                } else if( dribbbleType == 'user' ) {
                
                    $.jribbble.getShotsByPlayerId( dribbbleUsername , function (playerShots) {
                        var html = [];
                    
                        $.each(playerShots.shots, function (i, shot) {
                            html.push('<div class="flickr_badge_image"><a href="' + shot.url + '" target="_blank">');
                            html.push('<img src="' + shot.image_teaser_url + '" ');
                            html.push('alt="' + shot.title + '"></a></div>');
                        });
                    
                        $(dribbbleWrap).html(html.join(''));
                    }, {page: 1, per_page: Number(dribbbleCount)});
                
                } else if( dribbbleType == 'list' ) {
                
                    $.jribbble.getShotsByList( dribbbleList , function (listDetails) {
                        var html = [];
                    
                        $.each(listDetails.shots, function (i, shot) {
                            html.push('<div class="flickr_badge_image"><a href="' + shot.url + '" target="_blank">');
                            html.push('<img src="' + shot.image_teaser_url + '" ');
                            html.push('alt="' + shot.title + '"></a></div>');
                        });
                    
                        $(dribbbleWrap).html(html.join(''));
                    }, {page: 1, per_page: Number(dribbbleCount)});
                
                }
            
            });
            
            
        }
        

});


$(function() {
    

    
    
    siblingsFader();
    
    
    // Flex Slider
    
    

});