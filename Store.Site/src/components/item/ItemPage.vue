<template>
  <v-container grid-list-md>
    <v-layout row wrap v-if="item">
      <v-flex v-if="item.pictures" xs12 md4>
        <slider v-if="screens.length" :options="options" :items="screens">
          <div v-for="(picture, i) in screens" :key="i" :slot="`slide${i}`">
            <v-img :src="picture.href"></v-img>
          </div>
        </slider>
        {{ item.name }}:{{ item.cost.toFixed(2) }}
      </v-flex>
      <v-flex xs1>
        {{ item.description }}
        {{ item.shortDescription }}
      </v-flex>
    </v-layout>
  </v-container>
</template>
<script>
import { mapState, mapActions } from "vuex";
import slider from "../common/app-slider";

export default {
  data() {
    return {
      full: false
    };
  },
  components: {
    slider
  },
  created() {
    this.loadItem({ params: { itemNo: this.$route.params.itemId } });
  },
  computed: {
    screens() {
      return this.item.pictures.filter(x => ![1, 2].includes(x.type));
    },
    videoHref() {
      var a = this.item.items.find(
        x =>
          x.itemType == 0 &&
          [1, 2, 3, 4].includes(x.value.split(".").reverse()[0].length)
      );
      return a ? a.value : "";
    },
    ...mapState({
      item: state => state.items.item
    }),
    options() {
      return {
        spaceBetween: 0,
        loop: this.screens.length > 1,
        loopedSlides: this.screens.length, //looped slides should be the same
        slidesPerView: 1,
        navigation: {
          nextEl: ".swiper-button-next",
          prevEl: ".swiper-button-prev"
        }
      };
    }
  },
  methods: {
    getRate(type) {
      var a = this.item.rates.find(x => x.rateType == type);
      return a ? a : undefined;
    },
    ...mapActions({
      loadItem: "items/getItem"
    })
  }
};
</script>
