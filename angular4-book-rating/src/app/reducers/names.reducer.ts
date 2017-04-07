export let reducer = (aggregate, item) => {
  // if (aggregate[item] === undefined) {
  //   aggregate[item] = 1;
  // } else {
  //   aggregate[item]++;
  // }

  // aggregate[item] =
  //   aggregate[item] === undefined ?
  //     aggregate[item] = 1 :
  //     aggregate[item]++;

  // return aggregate;
  const counter = aggregate[item];
  const newAcc = aggregate.set(item, counter ? counter + 1 : 1);

  return newAcc;
};

