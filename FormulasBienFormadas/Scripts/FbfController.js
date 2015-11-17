(function () {
    'use strict';

    angular
        .module('app',[])
        .controller('FbfController', ["$scope", function FbfController($scope) {
            $scope.title = 'FbfController';
            $scope.line = "~n(X)";
            $scope.dataErrors = [];
            $scope.esFBF = false;
            $scope.analizar = function analizar() {
                $scope.esFBF = false;
                $.getJSON("/Home/Validate", { line: $scope.line }, function (data) {
                    if (data) {
                        console.log(data);
                        if (data.Errors) {
                            $scope.esFBF = data.Errors.length == 0;
                            if(!$scope.esFBF)
                                $scope.dataErrors = [data.Errors[0]];
                            else
                                $scope.dataErrors = []
                            
                            $scope.$apply()
                        }
                    }
                });
            }
        }]);

    
})();
