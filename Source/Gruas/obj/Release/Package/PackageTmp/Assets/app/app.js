var app = angular.module('app', [
    'ngRoute',
    'ngCookies',
    'notifyApp',
    'home',
    'signIn',
    'register',
    'reports',
    'menus',
    'createmenus',
    'solicituduser',
    'reportuser',
    'principal',
    'solgruas',
    'busquedasol',
    'solagents',
    'topegruas',
    'expsol',
    'listuser',
    'ans',
    'repseragente',
    'repservices',
    'RepServi',

]).filter('html', function ($sce) {
    return function (val) {
        return $sce.trustAsHtml(val);
    };
}).filter('startFromGrid', function () {
    return function (input, start) {
        start = +start;
        return input.slice(start);
    }
}).directive('onlynumbers', function () {
    return {
        require: 'ngModel',
        link: function (scope, element, attr, ngModelCtrl) {
            function fromUser(text) {
                if (text) {
                    var transformedInput = text.replace(/[^0-9]/g, '');

                    if (transformedInput !== text) {
                        ngModelCtrl.$setViewValue(transformedInput);
                        ngModelCtrl.$render();
                    }
                    return transformedInput;
                }
                return undefined;
            }
            ngModelCtrl.$parsers.push(fromUser);
        }
    };
})
    .directive('capitalize', function () {
        return {
            require: 'ngModel',
            link: function (scope, element, attrs, modelCtrl) {
                var capitalize = function (inputValue) {
                    if (inputValue == undefined) inputValue = '';
                    var capitalized = inputValue.toUpperCase();
                    if (capitalized !== inputValue) {
                        modelCtrl.$setViewValue(capitalized);
                        modelCtrl.$render();
                    }
                    return capitalized;
                }
                modelCtrl.$parsers.push(capitalize);
                capitalize(scope[attrs.ngModel]); // capitalize initial value
            }
        };
    })

    .directive('replaceval', function () {
        return {
            require: 'ngModel',
            link: function (scope, element, attr, ngModelCtrl) {
                function fromUser(text) {
                    var transformedInput = text.replace(/'/g, '-');
                    if (transformedInput !== text) {
                        ngModelCtrl.$setViewValue(transformedInput);
                        ngModelCtrl.$render();
                    }
                    return transformedInput;
                }
                ngModelCtrl.$parsers.push(fromUser);
            }
        };
    })

    .directive('onlyletters', function () {
        return {
            require: 'ngModel',
            link: function (scope, element, attr, ngModelCtrl) {
                function fromUser(text) {
                    var transformedInput = text.replace(/[^abcdefghijklmnñopqrstuvwxyzABCDEFGHIJKLMNÑOPQRSTUVWXYZ/1234567890ñÑáéíóúÁÉÍÓÚ &-]/g, '');
                    if (transformedInput !== text) {
                        ngModelCtrl.$setViewValue(transformedInput);
                        ngModelCtrl.$render();
                    }
                    return transformedInput;
                }
                ngModelCtrl.$parsers.push(fromUser);
            }
        };
    })

    .directive('onlysletters', function () {
        return {
            require: 'ngModel',
            link: function (scope, element, attr, ngModelCtrl) {
                function fromUser(text) {
                    var transformedInput = text.replace(/[^abcdefghijklmnñopqrstuvwxyzABCDEFGHIJKLMNÑOPQRSTUVWXYZ/ñÑáéíóúÁÉÍÓÚ &-]/g, '');
                    if (transformedInput !== text) {
                        ngModelCtrl.$setViewValue(transformedInput);
                        ngModelCtrl.$render();
                    }
                    return transformedInput;
                }
                ngModelCtrl.$parsers.push(fromUser);
            }
        };
    });




app.config(['$provide', '$routeProvider', '$httpProvider', function ($provide, $routeProvider, $httpProvider) {

    //================================================
    // Ignore Template Request errors if a page that was requested was not found or unauthorized.  The GET operation could still show up in the browser debugger, but it shouldn't show a $compile:tpload error.
    //================================================
    $provide.decorator('$templateRequest', ['$delegate', function ($delegate) {
        var mySilentProvider = function (tpl, ignoreRequestError) {
            return $delegate(tpl, true);
        }
        return mySilentProvider;
    }]);

    //================================================
    // Add an interceptor for AJAX errors
    //================================================
    $httpProvider.interceptors.push(['$q', '$location', function ($q, $location) {
        return {
            'responseError': function (response) {
                if (response.status === 401)
                    $location.url('/signin');
                return $q.reject(response);
            }
        };
    }]);


    //================================================
    // Routes
    //===============================================
    
    $routeProvider.when('/register', {
        templateUrl: 'App/Register',
        controller: 'registerCtrl'
    }).when('/signin/:message?', {
        templateUrl: 'App/SignIn',
        controller: 'signInCtrl'
    }).when('/principal', {
        templateUrl: 'Principal/Principal',
        controller: 'PrincipalCtrl'
    });

    var ListMenu = JSON.parse(localStorage.getItem("_Routes"));   

    angular.forEach(ListMenu, function (route) {
        angular.forEach(route.SubMenu, function (Subroute) {
            $routeProvider.when(Subroute.AUrl, { templateUrl: Subroute.ATemplateUrl, controller: Subroute.AController });
        });
    });
    
    $routeProviderReference = $routeProvider;

    $routeProviderReference.otherwise({ redirectTo: '/signin' });
}]);


app.config(['growlProvider', function (growlProvider) {
    growlProvider.globalTimeToLive(3000);
    //growlProvider.messagesKey("my-messages");
    //growlProvider.messageTextKey("messagetext");
    //growlProvider.messageSeverityKey("severity-level");
    //growlProvider.onlyUniqueMessages(true);

}]);
app.run(['$http', '$cookies', '$cookieStore', function ($http, $cookies, $cookieStore) {
    //If a token exists in the cookie, load it after the app is loaded, so that the application can maintain the authenticated state.
    $http.defaults.headers.common.Authorization = 'Bearer ' + $cookieStore.get('_Token');
    $http.defaults.headers.common.RefreshToken = $cookieStore.get('_RefreshToken');
}]);


//GLOBAL FUNCTIONS - pretty much a root/global controller.
//Get username on each page
//Get updated token on page change.
//Logout available on each page.
app.run(['$rootScope', '$http', '$cookies', 'growl', '$cookieStore', '$templateCache','$route', function ($rootScope, $http, $cookies, growl, $cookieStore, $templateCache, $route) {
    
    $rootScope.ListMenuLog = JSON.parse(localStorage.getItem("_Routes"));
    
    $rootScope.intro = true;

    $rootScope.logout = function () {

        $http.post('/api/Account/Logout')
            .success(function (data, status, headers, config) {

                $http.defaults.headers.common.Authorization = null;
                $http.defaults.headers.common.RefreshToken = null;
                $cookieStore.remove('_Token');
                $cookieStore.remove('_RefreshToken');                
                $rootScope.username = '';
                $rootScope.loggedIn = false;
                $rootScope.intro = true;
                $rootScope.ListMenuLog = null;
                window.location = '#/signin';
                $templateCache.removeAll();
            });

    }

    $rootScope.$on('$locationChangeSuccess', function (event) {
        if ($http.defaults.headers.common.RefreshToken != null) {
            var params = "grant_type=refresh_token&refresh_token=" + $http.defaults.headers.common.RefreshToken;
            $http({
                url: '/Token',
                method: "POST",
                headers: { 'Content-Type': 'application/x-www-form-urlencoded' },
                data: params
            })
                .success(function (data, status, headers, config) {
                    $http.defaults.headers.common.Authorization = "Bearer " + data.access_token;
                    $http.defaults.headers.common.RefreshToken = data.refresh_token;

                    $cookieStore.put('_Token', data.access_token);
                    $cookieStore.put('_RefreshToken', data.refresh_token);
                    $rootScope.IdUser = data.id;
                    $rootScope.userEmail = data.userEmail;
                    $rootScope.userfirst = data.userfirst;
                    $rootScope.Role = data.Role;
                    $rootScope.AccessFailedCount = data.AccessFailedCount;

                    $http.get('/api/WS_Account/GetCurrentUserName')
                        .success(function (data, status, headers, config) {
                            if (data != "null") {
                                $rootScope.username = data.replace(/["']{1}/gi, "");//Remove any quotes from the username before pushing it out.
                                $rootScope.loggedIn = true;
                                $rootScope.intro = false;
                                $rootScope.GetMenuLogin();
                            }
                            else {
                                $rootScope.loggedIn = false;
                                $rootScope.intro = true;
                                window.location = '#/signin';
                            }

                            //alert('asd');
                        });


                })
                .error(function (data, status, headers, config) {
                    //alert('fuera');
                    growl.error("La Sesión Expiro.", { title: 'Error!' });
                    $rootScope.loggedIn = false;
                    $rootScope.logout();
                });
        }

        $rootScope.GetMenuLogin = function () {           

            if ($rootScope.AccessFailedCount == 1) {
                $rootScope.opnemodal();
            }

        }


        $rootScope.opnemodal = function () {
            $("#modalpaslogin").modal({
                backdrop: 'static',
                keyboard: false
            })
        }


        $rootScope.actualizar = function () {

            $rootScope.val.Id = $rootScope.IdUser;
            $rootScope.val.Email = $rootScope.userEmail;
            $rootScope.val.firstName = $rootScope.userfirst;

            $http.post('/api/Account/ChangePass', $rootScope.val)
                .success(function (data, status, headers, config) {
                    $rootScope.successMessage = "Contraseña Actualizada Correctamente";
                    growl.success($rootScope.successMessage, { title: 'Success!' });

                    $rootScope.val.OldPassword = "";
                    $rootScope.val.NewPassword = "";
                    $rootScope.val.ConfirmPassword = "";

                    $('#modalpaslogin').modal('hide');
                })
                .error(function (data, status, headers, config) {
                    growl.error(data, { title: 'Error!' });
                });
        }

        $rootScope.showdata = function () {
            $(".userdata").toggle('slow');
        }

    });
}]);

