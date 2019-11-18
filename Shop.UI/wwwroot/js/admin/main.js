var app = new Vue({
    el: '#app',
    data: {
        price: 0,
        showPrice: true,
    },
    methods: {
        togglePrice: function() {
            this.showPrice = !this.showPrice;
        }
    }
    computed: {
        formatPrice: function() {
            return "$" + this.price;
        }
    }
})