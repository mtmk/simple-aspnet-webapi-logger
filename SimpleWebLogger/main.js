
angular.module('simpleLogger', [])
    .controller('logCtrl', function($scope, $http) {

        $scope.message = '';
        $scope.reply = '<server reply comes here>';
        $scope.level = 'DEBUG';

        $scope.log = function() {
            $http({
                method: 'POST',
                url: '/api/log',
                data: $scope.message,
                headers: {
                    'Content-Type': 'text/plain',
                    'X-LogLevel': $scope.level
                }
            }).success(function(data) {
                $scope.reply = data;
            });
        };

        $scope.deleteLog = function() {
            $http({
                method: 'DELETE',
                url: '/api/log',
            }).success(function(data) {
                $scope.reply = data;
            });
        };

    });


